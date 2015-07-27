using System;
using System.ComponentModel;

namespace NParametrizer.Tests
{
	internal class TestParametersClass : ParametersBase
	{
		private string _ftpUriString;

		public TestParametersClass(string[] args) : base(args)
		{
		}

		/// <summary>
		///   Ftp/sftp URI string
		/// </summary>
		[Parameter("--F", Description = "Ftp/sftp complete uri including credentials")]
		[Config("FTPUri", ConfigType.AppSettings)]
		public string FtpUriString
		{
			get { return _ftpUriString; }
			set
			{
				FtpUri = new UriInfo(value);
				_ftpUriString = FtpUri.ToString();
			}
		}

		/// <summary>
		///   Ftp/sftp complete uri including credentials
		/// </summary>
		public UriInfo FtpUri { get; private set; }

		[Parameter("all", Description = "Import all parts")]
		[DefaultValue(false)]
		public bool ImportAll
		{
			get { return Import1 && Import2 && Import3; }
			set { Import1 = Import2 = Import3 = value; }
		}

		/// <summary>
		///   Process radioshack files
		/// </summary>
		[Parameter("import1", Description = "Process import 1")]
		[DefaultValue(false)]
		public bool Import1 { get; private set; }

		/// <summary>
		///   Process SSIS workflows
		/// </summary>
		[Parameter("import2", Description = "Process import 2")]
		[DefaultValue(false)]
		public bool Import2 { get; set; }

		/// <summary>
		///   Import Sweep file
		/// </summary>
		[Parameter("import3", Description = "Process import 3")]
		[DefaultValue(false)]
		public bool Import3 { get; set; }

		/// <summary>
		///   Perform download from FTP server or use local files in <see cref="DownloadPath" /> path. Default value is
		///   <value>True</value>
		/// </summary>
		[Parameter("download",
			Description =
				"Perform download from FTP server or use local files in [DownloadPath] path. Default value is [True]")]
		[DefaultValue(false)]
		public bool PerformFtpDownload { get; set; }

		/// <summary>
		///   Maximum iteration occure for wait until destination file is available to move to
		/// </summary>
		[Parameter("--mI", "--MaxIteration",
			Description = "Maximum iteration occure for wait until destination file is available to move to")]
		[DefaultValue(30)]
		public int MaxIteration { get; set; }

		/// <summary>
		///   Time in milliseconds how long the process waits until to try write in next iteration.
		///   Default value i 60 seconds (60000 milliseconds)
		/// </summary>
		[Parameter("--fL", "--FileLockWait",
			Description = "Maximum iteration occure for wait until destination file is available to move to")]
		[DefaultValue(60000)]
		public int LockWait { get; set; }

		/// <summary>
		///   File name for sweep data process. If not specified default value is
		///   <value>Files\Samples\ in folder where tool is currently running</value>
		///   Value can be defined in appSettings["DownloadPath"] or -DownloadPath|-d={Directory Name} command-line parameter
		/// </summary>
		[Parameter("--d", "--DownloadPath",
			Description = "File name for sweep data process. If not specified default value is"
			              + "[Files\\Samples] in folder where tool is currently running. "
			              +
			              "Value can be defined in appSettings[DownloadPath] or -DownloadPath|-d={Directory Name} command-line parameter"
			)]
		[DefaultValue("Files\\Samples")]
		[Config("DownloadPath", ConfigType.AppSettings)]
		public string DownloadPath { get; set; }

		/// <summary>
		///   File name for wireless pnals pricing data. If not specified default value is
		///   <value>WirelessPlanPricing.txt</value>
		///   Value can be defined in appSettings["WirelessPlanPricingFileName"] or -WirelessPlanPricingFileName|-wpp={File Name}
		///   command-line parameter
		/// </summary>
		[Parameter("--sf", "--SuperFileToImport", Description = "")]
		[DefaultValue("SuperDataToImport.txt")]
		[Config("SuperFileToImport", ConfigType.AppSettings)]
		public string SuperFileToImport { get; set; }

		/// <summary>
		///   SqlServer database connection string
		/// </summary>
		[Parameter("--S", Description = "")]
		[Config("SqlServer", ConfigType.ConnectionString)]
		public string SqlServerConnectionString { get; set; }

		/// <summary>
		///   Mongo database connection string
		/// </summary>
		[Parameter("--M", Description = "")]
		[Config("MongoServer", ConfigType.ConnectionString)]
		public string MongoServerString { get; set; }

		[Parameter("--ENMN", Description = "Test enum value")]
		public TestEnum? EnumNullableValue { get; set; }

		[Parameter("--ENM", Description = "Test enum value")]
		public TestEnum EnumValue { get; set; }

		protected override void ValidateArguments()
		{
			if (FtpUri == null)
			{
				throw new NullReferenceException(
					"FTP connection is not provided. Use appSettings[FtpUri] or command line param -F={FULL URI}");
			}

			if (SqlServerConnectionString == null)
			{
				throw new NullReferenceException(
					"SQL SERVER connection string is not provided. Use connectionStrings[SqlServer] or command line param -S={SQL SERVER CONNECTION STRING}");
			}

			if (MongoServerString == null)
			{
				throw new NullReferenceException(
					"MONGO conneCtion string is not provided. Use connectionStrings[SSISSqlServer] or command line param -I={SSIS CONNECTION STRING}");
			}
		}
	}
}