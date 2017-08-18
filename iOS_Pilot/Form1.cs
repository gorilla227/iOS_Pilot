using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;

namespace iOS_Pilot
{
	public partial class Form1 : Form
	{
		public Form1 ()
		{
			InitializeComponent();
		}

		private void Form1_Load ( object sender, EventArgs e )
		{
			NativeLibraries.Load();
			LoadDevices();
		}

		private void LoadDevices ()
		{
			ReadOnlyCollection<string> udids;
			int count = 0;

			var idevice = LibiMobileDevice.Instance.iDevice;
			var lockdown = LibiMobileDevice.Instance.Lockdown;

			var ret = idevice.idevice_get_device_list( out udids, ref count );

			if ( ret == iDeviceError.NoDevice )
			{
				// Not actually an error in our case
				return;
			}

			ret.ThrowOnError();

			// Get the device name
			foreach ( var udid in udids )
			{
				iDeviceHandle deviceHandle;
				idevice.idevice_new( out deviceHandle, udid ).ThrowOnError();

				LockdownClientHandle lockdownHandle;
				lockdown.lockdownd_client_new_with_handshake( deviceHandle, out lockdownHandle, "Quamotion" ).ThrowOnError();

				string deviceName;
				lockdown.lockdownd_get_device_name( lockdownHandle, out deviceName ).ThrowOnError();

				deviceHandle.Dispose();
				lockdownHandle.Dispose();
			}
		}

	}
}