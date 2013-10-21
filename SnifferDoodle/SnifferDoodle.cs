using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace SnifferDoodle
{
	public partial class SnifferDoodle : Form
	{
		private StringBuilder inputBuffer = new StringBuilder();
		private static char[] cmdDelim = new char[] { ':' };

		public delegate void SimpleDelegate();
		private SimpleDelegate ReadDataDelegate;

		private int maxHistory = 10000;
		private DateTime startCapture;
		private Regex filter = null;
		private bool ignoreResponse = false;

		public SnifferDoodle()
		{
			InitializeComponent();

			ReadDataDelegate = new SimpleDelegate(readSnifferData);
		}

		private void SnifferDoodle_Load(object sender, EventArgs e)
		{
			this.ClientSize = Properties.Settings.Default.AppSize;
			this.Location = Properties.Settings.Default.AppLocation;

			foreach (string port in SerialPort.GetPortNames())
			{
				portComboList.Items.Add(port);
			}

			portComboList.SelectedItem = global::SnifferDoodle.Properties.Settings.Default["Port"];

			maxHistory = (int)global::SnifferDoodle.Properties.Settings.Default["History"];
			maxHistoryEditBox.ValidatingType = typeof(int);
			maxHistoryEditBox.Text = maxHistory.ToString();

			notBox.Checked = (bool)global::SnifferDoodle.Properties.Settings.Default["Not"];
		}

		private void portComboList_SelectedValueChanged(object sender, EventArgs e)
		{
			snifferPort.PortName = portComboList.SelectedItem.ToString();
		}

		private void captureButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (snifferPort.IsOpen == false)
				{
					string filterText = captureFilter.Text.Trim();

					if (filterText.Length > 0)
					{
						filter = new Regex(filterText);
					}
					else
					{
						filter = null;
					}

					// Open the serial port
					snifferPort.Open();

					// Tell the sniffer to start sniffing
					snifferPort.Write("R\r");

					// Mark the start of this capture session
					startCapture = DateTime.Now;
				}
				else
				{
					// Tell the sniffer to stop sniffing
					snifferPort.Write("T\r");

					// Close the serial port
					snifferPort.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			if (snifferPort.IsOpen == true)
			{
				captureButton.Text = "Stop";
				portComboList.Enabled = false;
				captureFilter.Enabled = false;
				maxHistoryEditBox.Enabled = false;
				notBox.Enabled = false;
			}
			else
			{
				captureButton.Text = "Start";
				portComboList.Enabled = true;
				captureFilter.Enabled = true;
				maxHistoryEditBox.Enabled = true;
				notBox.Enabled = true;
			}
		}

		private void snifferPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (e.EventType == SerialData.Chars)
			{
				this.BeginInvoke(ReadDataDelegate);
			}
		}

		private void readSnifferData()
		{
			bool addedItem = false;
			bool autoScroll = false;

			if (snifferDataList.Items.Count > 0)
			{
				autoScroll = (snifferDataList.GetItemRect(snifferDataList.Items.Count - 1).Y < snifferDataList.Bottom);
			}

			// Read in the data
			byte[] buff = new byte[1024];
			int cnt;
			char ch;
			while (snifferPort.BytesToRead > 0)
			{
				cnt = snifferPort.Read(buff, 0, snifferPort.BytesToRead > buff.Length ? buff.Length : snifferPort.BytesToRead);
				for (int i = 0; i < cnt; i++)
				{
					ch = (char)buff[i];
					if ((ch == '\r') || (ch == '\n'))
					{
						if (inputBuffer.Length > 0)
						{
							// Process this data
							addedItem |= processSnifferData(inputBuffer.ToString(), snifferDataList.Items);

							inputBuffer.Length = 0;
						}
					}
					else
					{
						inputBuffer.Append(ch);
					}
				}
			}

			if ((addedItem == true) && (autoScroll == true) && (snifferDataList.Items.Count > 0))
			{
				snifferDataList.EnsureVisible(snifferDataList.Items.Count - 1);
			}
		}

		private bool processSnifferData(string data, ListView.ListViewItemCollection newItems)
		{
			// Look at what we've got
			string[] parts = data.Split(cmdDelim);
			bool addedItem = false;

			if (parts.Length == 2)
			{
				string dataType = parts[0].Trim();
				string dataData = parts[1].Trim();

				if ((dataType == "R") || (dataType == "A"))
				{
					if ((ignoreResponse == false) && (newItems.Count > 0))
					{
						ListViewItem lastItem = null;

						if (newItems.Count > 0)
						{
							lastItem = newItems[newItems.Count - 1];
						}
						else if (snifferDataList.Items.Count > 0)
						{
							lastItem = snifferDataList.Items[snifferDataList.Items.Count - 1];
						}

						if (lastItem != null)
						{
							lastItem.SubItems.Add(dataData);

							// Attempt to decode the data into meaningful information
							FormatDetails(lastItem);
						}
					}

					ignoreResponse = false;
				}
				else
				{
					if ((filter == null) || ((filter.IsMatch(dataData) == true) ^ notBox.Checked))
					{
						while (newItems.Count > maxHistory)
						{
							newItems.RemoveAt(0);
						}

						TimeSpan timeStamp = DateTime.Now - startCapture;

						ListViewItem newItem = new ListViewItem(timeStamp.TotalSeconds.ToString("0.000000000"));

						newItem.SubItems.Add(dataType);
						newItem.SubItems.Add(dataData);

						newItems.Add(newItem);

						addedItem = true;
					}
					else
					{
						ignoreResponse = true;
					}
				}
			}
			else if (data == "GET24?")
			{
				if ((filter == null) || ((filter.IsMatch(data) == true) ^ notBox.Checked))
				{
					while (newItems.Count > maxHistory)
					{
						newItems.RemoveAt(0);
					}

					TimeSpan timeStamp = DateTime.Now - startCapture;

					ListViewItem newItem = new ListViewItem(timeStamp.TotalSeconds.ToString("0.000000000"));

					newItem.SubItems.Add(data);

					newItems.Add(newItem);

					addedItem = true;
				}
				else
				{
					ignoreResponse = true;
				}
			}
			else
			{
				// Some sort of problem parsing data
				Console.WriteLine("Unprocessed sniffer data: {0}", data);
			}

			return addedItem;
		}

		private string GetErrorString(byte error)
		{
			switch (error)
			{
				case 0x81: return "Board Busy";
				case 0x82: return "Command Busy";
				case 0x83: return "Bad Checksum";
				case 0x84: return "Bad Command";
				case 0x85: return "Bad Parameter";
			}

			return "Success";
		}

		private void FormatDetails(ListViewItem item)
		{
			if (item.SubItems.Count < 3) return;

			try
			{
				// Create byte arrays from the data
				string[] sndBytesString = item.SubItems[2].Text.Split(' ');
				string[] rcvBytesString = (item.SubItems.Count > 3) ? item.SubItems[3].Text.Split(' ') : new string[0];

				byte[] sndBytes = new byte[sndBytesString.Length];
				byte[] rcvBytes = new byte[rcvBytesString.Length];

				try
				{
					for (int i = 0; i < sndBytes.Length; i++) sndBytes[i] = byte.Parse(sndBytesString[i], System.Globalization.NumberStyles.AllowHexSpecifier);
					for (int i = 0; i < rcvBytes.Length; i++) rcvBytes[i] = byte.Parse(rcvBytesString[i], System.Globalization.NumberStyles.AllowHexSpecifier);
				}
				catch(Exception)
				{
					return;
				}

				string details = "???";

				// Is this a primary command or extended
				if ((sndBytes[0] & 0xC0) == 0xC0)
				{
					switch (sndBytes[1])
					{
						case 0x05:
							if (rcvBytes.Length == 4) details = string.Format("GetSensor() = {0}", (rcvBytes[0] << 16) | (rcvBytes[1] << 8) | rcvBytes[2]);
							else details = string.Format("GetSensor() == Error");
							break;
						case 0x10:
							if (sndBytes.Length == 10) details = string.Format("SetRange(false, {0}, {1}) = ", (sndBytes[2] << 16) | (sndBytes[3] << 8) | sndBytes[4], (sndBytes[5] << 16) | (sndBytes[6] << 8) | sndBytes[7]);
							else if (sndBytes.Length == 8) details = string.Format("SetRange(true, {0}, {1}) = ", (sndBytes[3]), (sndBytes[4] << 8) | sndBytes[5]);
							else details = string.Format("SetRange(???) = ");
							if (rcvBytes.Length > 0) details += GetErrorString(rcvBytes[0]);
							break;
						case 0x13:
							if (rcvBytes.Length == 2) details = string.Format("GetVersion() = {0}.{1}", rcvBytes[0] >> 4, rcvBytes[0] & 0x0F);
							else details = string.Format("GetVersion() = Error");
							break;
						case 0x14:
							if (sndBytes.Length == 5) details = string.Format("SetStTimeout({0}) = ", sndBytes[2]);
							else if (sndBytes.Length == 7) details = string.Format("SetStTimeout({0}, {1}) = ", (sndBytes[2] << 8) | sndBytes[3], sndBytes[4]);
							if (rcvBytes.Length > 0) details += GetErrorString(rcvBytes[0]);
							break;
						case 0x92:
							if (sndBytes.Length == 5) details = string.Format("GetLoad({0}) = ", sndBytes[2]);
							if (rcvBytes.Length > 1) details += string.Format("{0}", rcvBytes[0]);
							else if (rcvBytes.Length == 1) details += GetErrorString(rcvBytes[0]);
							break;
						case 0x95:
							if (sndBytes.Length == 12) details = string.Format("SetLinearization({0}, {1}, {2}, {3}, {4}, {5}) = ",
								(sndBytes[2] & 0x80) != 0,
								(sndBytes[2] & 0x7F), 
								sndBytes[3],
								(sndBytes[4] << 8) | sndBytes[5],
								(sndBytes[6] << 8) | sndBytes[7],
								(sndBytes[8] << 8) | sndBytes[9]);
							else if (sndBytes.Length == 9) details = string.Format("SetLinearization({0}, {1}, {2}, {3}, {4}) = ",
								(sndBytes[2] & 0x80) != 0,
								(sndBytes[2] >> 4) & 0x0F,
								(sndBytes[2] & 0x03),
								(sndBytes[3] << 8) | sndBytes[4],
								(sndBytes[5] << 8) | sndBytes[6]);
							else if (sndBytes.Length == 5) details = string.Format("SetLinearization({0}, {1}) = ",
								(sndBytes[2] & 0x80) != 0,
								(sndBytes[2] & 0x7F));
							if (rcvBytes.Length > 0) details += GetErrorString(rcvBytes[0]);
							break;
						case 0xB0:
							if (rcvBytes.Length > 1)
							{
								StringBuilder builder = new StringBuilder();
								builder.Append("GetRS232Data() = [");
								for (int i = 0; i < rcvBytes.Length - 1; i++)
								{
									if ((rcvBytes[i] < 0x20) || (rcvBytes[i] > 0x7D))
									{
										builder.Append('{');
										builder.Append(rcvBytes[i].ToString("X2"));
										builder.Append('}');
									}
									else
									{
										builder.Append((char)rcvBytes[i]);
									}
								}
								builder.Append("]");
								details = builder.ToString();
							}
							else
							{
								details = string.Format("GetRS232Data() = {0}", GetErrorString(rcvBytes[0]));
							}
							break;
						case 0xB1:
							if (rcvBytes.Length > 1)
							{
								StringBuilder builder = new StringBuilder();
								builder.Append("SendRS232Data(\"");
								for (int i = 2; i < sndBytes.Length - 2; i++)
								{
									if ((sndBytes[i] < 0x20) || (sndBytes[i] > 0x7D))
									{
										builder.Append('{');
										builder.Append(sndBytes[i].ToString("X2"));
										builder.Append('}');
									}
									else
									{
										builder.Append((char)sndBytes[i]);
									}
								}
								builder.Append("\") = ");
								builder.Append((int)rcvBytes[0]);
								details = builder.ToString();
							}
							else
							{
								details = string.Format("SendRS232Data() = {0}", GetErrorString(rcvBytes[0]));
							}
							break;
						case 0xE5:
							if (sndBytes.Length == 6) details = string.Format("ConfigureLoad({0}, {1}, {2}) = ", sndBytes[2], (sndBytes[3] & 0x02) != 0, (sndBytes[3] & 0x01) != 0);
							if (rcvBytes.Length > 0) details += GetErrorString(rcvBytes[0]);
							break;
					}
				}
				else
				{
					switch ((sndBytes[0] & 0xC0) >> 6)
					{
						case 0:
							if (rcvBytes.Length == 3) details = string.Format("GetSwitch() = {0,4:X}", (rcvBytes[0] << 8) | (rcvBytes[1]));
							else details = string.Format("GetSwitch() = Error");
							break;
						case 1:
							break;
						case 2:
							break;
					}
				}
				item.SubItems.Add(details);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private void SnifferDoodle_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (snifferPort.IsOpen == true)
			{
				captureButton_Click(null, null);
			}

			global::SnifferDoodle.Properties.Settings.Default.Save();
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			snifferDataList.Items.Clear();
		}

		private void maxHistoryEditBox_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
		{
			if (e.IsValidInput == true)
			{
				maxHistory = (int)e.ReturnValue;

				global::SnifferDoodle.Properties.Settings.Default["History"] = maxHistory;
			}
		}

		private void portComboList_SelectedIndexChanged(object sender, EventArgs e)
		{
			global::SnifferDoodle.Properties.Settings.Default["Port"] = (string)portComboList.SelectedItem;
		}

		private void SnifferDoodle_FormClosed(object sender, FormClosedEventArgs e)
		{
			Properties.Settings.Default.Save();
		}

		private void SnifferDoodle_ResizeEnd(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Normal)
			{
				Properties.Settings.Default.AppSize = this.ClientSize;
				Properties.Settings.Default.AppLocation = this.Location;
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StringBuilder command = new StringBuilder();
			foreach (ListViewItem item in snifferDataList.SelectedItems)
			{
				command.AppendFormat("{0,14} | ", item.SubItems[0].Text);
				command.AppendFormat("{0,-6} | ", item.SubItems[1].Text);
				command.AppendFormat("{0,-53} | ", item.SubItems[2].Text);
				if (item.SubItems.Count > 3) command.AppendFormat("{0,-44} | ", item.SubItems[3].Text);
				if (item.SubItems.Count > 4) command.Append(item.SubItems[4].Text);
				command.Append("\n");
			}
			Clipboard.SetText(command.ToString());
		}

		private void snifferDataList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			snifferDataContextMenu.Items[0].Enabled = (snifferDataList.SelectedItems.Count > 0);

			if (snifferDataList.SelectedItems.Count > 0)
			{
				// Compute the time difference between the first selected and last selected packet
				double ts1 = double.Parse(snifferDataList.SelectedItems[0].SubItems[0].Text);
				double ts2 = double.Parse(snifferDataList.SelectedItems[snifferDataList.SelectedItems.Count - 1].SubItems[0].Text);

				double span = ts2 - ts1;

				if (span > 0)
				{
					packetTimeDiff.Text = string.Format("Span: {0} seconds", span);
				}
				else
				{
					packetTimeDiff.Text = "";
				}
			}
			else
			{
				packetTimeDiff.Text = "";
			}
		}

		private void notBox_CheckStateChanged(object sender, EventArgs e)
		{
			global::SnifferDoodle.Properties.Settings.Default["Not"] = notBox.Checked;
		}
	}
}