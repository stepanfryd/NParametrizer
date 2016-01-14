using NUnit.Framework;

namespace NParametrizer.Tests
{
	[TestFixture]
	public class NParametrizerTests
	{
		private const string SUPER_FILE_TO_IMPORT = "TEST_SUPERFILE_FILE_NAME";
		private const string DOWNLOAD_PATH = "TEST_DIRECTORY_PATH";
		private const string FTP_URI_HOST = "test.server.test";
		private const string FTP_URI_PASSWORD = "test-password";
		private const string FTP_URI_PATH = "/Test/Path";
		private const int FTP_URI_PORT = 22;
		private const string FTP_URI_SCHEME = "sftp";
		private const string FTP_URI_USER = "test-user";
		private const string SQL_SERVER_CONNECTION_STRING = "TEST_SQLSERVER_CONNECTION_STRING_APP.CONFIG";
		private const string MONGO_SERVER_CONNECTION_STRING = "TEST_MONGO_CONNECTION_STRING_APP.CONFIG";

		private string FtpUriFull
			=> $"{FTP_URI_SCHEME}://{FTP_URI_USER}:{FTP_URI_PASSWORD}@{FTP_URI_HOST}:{FTP_URI_PORT}{FTP_URI_PATH}";

		[Test]
		public void AllFilesTest()
		{
			var par = new TestParametersClass(new[] {"all", "download"});

			Assert.IsTrue(par.Import1);
			Assert.IsTrue(par.Import2);
			Assert.IsTrue(par.Import3);
			Assert.IsTrue(par.PerformFtpDownload);
		}

		[Test]
		public void AllTest()
		{
			var par = new TestParametersClass(new[] {"all"});

			Assert.IsTrue(par.Import1);
			Assert.IsTrue(par.Import2);
			Assert.IsTrue(par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
		}

		[Test]
		public void AppSettingsTest()
		{
			var par = new TestParametersClass(new string[] {});

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FTP_URI_HOST);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FTP_URI_USER);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FTP_URI_PATH);
			Assert.IsTrue(par.FtpUri.Port == FTP_URI_PORT);
			Assert.IsTrue(par.FtpUri.Scheme == FTP_URI_SCHEME);
			Assert.IsTrue(par.DownloadPath == DOWNLOAD_PATH);
			Assert.IsTrue(par.SqlServerConnectionString == SQL_SERVER_CONNECTION_STRING);
			Assert.IsTrue(par.MongoServerString == MONGO_SERVER_CONNECTION_STRING);
		}

		[Test]
		public void ChangeFtpUriTest()
		{
			var par =
				new TestParametersClass(new[] {"--F=ftp://new-user:new-password@new.testserver.test:23/Totally/New/Path"});

			Assert.IsTrue(par.FtpUri.ToString() == "ftp://new-user:new-password@new.testserver.test:23/Totally/New/Path");
			Assert.IsTrue(par.FtpUri.Host == "new.testserver.test");
			Assert.IsTrue(par.FtpUri.Credentials.UserName == "new-user");
			Assert.IsTrue(par.FtpUri.Credentials.Password == "new-password");
			Assert.IsTrue(par.FtpUri.AbsolutePath == "/Totally/New/Path");
			Assert.IsTrue(par.FtpUri.Port == 23);
			Assert.IsTrue(par.FtpUri.Scheme == "ftp");

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.SuperFileToImport == SUPER_FILE_TO_IMPORT);
			Assert.IsTrue(par.DownloadPath == DOWNLOAD_PATH);
			Assert.IsTrue(par.SqlServerConnectionString == SQL_SERVER_CONNECTION_STRING);
			Assert.IsTrue(par.MongoServerString == MONGO_SERVER_CONNECTION_STRING);
		}

		[Test]
		public void ConnectionStringsTest()
		{
			var newSql = "NEW_SQL_CONNECTION_STRING";
			var newMongo = "NEW_SSIS_CONNECTION_STRING";

			var par = new TestParametersClass(new[] {$"--S={newSql}", $"--M={newMongo}"});

			Assert.IsTrue(par.SqlServerConnectionString == newSql);
			Assert.IsTrue(par.MongoServerString == newMongo);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FTP_URI_HOST);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FTP_URI_USER);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FTP_URI_PATH);
			Assert.IsTrue(par.FtpUri.Port == FTP_URI_PORT);
			Assert.IsTrue(par.FtpUri.Scheme == FTP_URI_SCHEME);
			Assert.IsTrue(par.SuperFileToImport == SUPER_FILE_TO_IMPORT);
			Assert.IsTrue(par.DownloadPath == DOWNLOAD_PATH);
		}

		[Test]
		public void CustomSectionTest()
		{
			var config = new TestParametersClass();

			Assert.IsTrue(config.CustomConfig.ConfigElement == "ConfigElementValue");
			Assert.IsTrue(config.CustomConfig.Number == 100);
			Assert.IsTrue(config.CustomConfigGroup.ConfigElement == "ConfigGroupElementValue");
			Assert.IsTrue(config.CustomConfigGroup.Number == 200);
		}

		[Test]
		public void DisableDownloadTest()
		{
			var par = new TestParametersClass(new[] {"download"});

			Assert.IsTrue(par.PerformFtpDownload);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FTP_URI_HOST);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FTP_URI_USER);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FTP_URI_PATH);
			Assert.IsTrue(par.FtpUri.Port == FTP_URI_PORT);
			Assert.IsTrue(par.FtpUri.Scheme == FTP_URI_SCHEME);
			Assert.IsTrue(par.SuperFileToImport == SUPER_FILE_TO_IMPORT);
			Assert.IsTrue(par.DownloadPath == DOWNLOAD_PATH);
			Assert.IsTrue(par.SqlServerConnectionString == SQL_SERVER_CONNECTION_STRING);
			Assert.IsTrue(par.MongoServerString == MONGO_SERVER_CONNECTION_STRING);
		}

		[Test]
		public void EnumTest()
		{
			var par = new TestParametersClass(new[] {"--ENM=Value1"});
			Assert.IsTrue(par.EnumValue.Equals(TestEnum.Value1));

			var par1 = new TestParametersClass(new[] {"--ENM=Value3"});
			Assert.IsTrue(par1.EnumValue.Equals(TestEnum.Value3));

			var par2 = new TestParametersClass(new[] {"--AA=something"});
			Assert.IsTrue(par2.EnumNullableValue.Equals(null));

			var par3 = new TestParametersClass(new[] {"--ENMN=Value3"});
			Assert.IsTrue(par3.EnumNullableValue.Equals(TestEnum.Value3));
		}

		[Test]
		public void EnvironmentVarsTest()
		{

			//var par = new TestParametersClass();
			//Assert.IsTrue(par.TestParamUnspecified == 100000);
			//Assert.IsFalse(par.TestParamUnspecified == 200000);
			//Assert.IsTrue(par.TestParamUser == "USER_ONLY_VALUE");
			//Assert.IsTrue(par.TestParamMachine == "MACHINE_ONLY_VALUE");
		}

		[Test]
		public void FilesLongChangePrefixTests()
		{
			var newDownloadPath = "NEW_DOWNLOAD_PATH";
			var newSuperFile = "NEW_SUPER_FILE";

			var par = new TestParametersClass(new[]
			{
				$"--DownloadPath={newDownloadPath}",
				$"--SuperFileToImport={newSuperFile}"
			});

			Assert.IsFalse(par.DownloadPath == DOWNLOAD_PATH);
			Assert.IsTrue(par.DownloadPath == newDownloadPath);

			Assert.IsFalse(par.SuperFileToImport == SUPER_FILE_TO_IMPORT);
			Assert.IsTrue(par.SuperFileToImport == newSuperFile);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FTP_URI_HOST);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FTP_URI_USER);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FTP_URI_PATH);
			Assert.IsTrue(par.FtpUri.Port == FTP_URI_PORT);
			Assert.IsTrue(par.FtpUri.Scheme == FTP_URI_SCHEME);
			Assert.IsTrue(par.SqlServerConnectionString == SQL_SERVER_CONNECTION_STRING);
			Assert.IsTrue(par.MongoServerString == MONGO_SERVER_CONNECTION_STRING);
		}

		[Test]
		public void FilesLongTests()
		{
			var newDownloadPath = "NEW_DOWNLOAD_PATH";
			var newSuperFile = "NEW_SUPER_FILE";

			var par = new TestParametersClass(new[]
			{
				$"--DownloadPath={newDownloadPath}",
				$"--SuperFileToImport={newSuperFile}"
			});

			Assert.IsFalse(par.DownloadPath == DOWNLOAD_PATH);
			Assert.IsTrue(par.DownloadPath == newDownloadPath);

			Assert.IsFalse(par.SuperFileToImport == SUPER_FILE_TO_IMPORT);
			Assert.IsTrue(par.SuperFileToImport == newSuperFile);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FTP_URI_HOST);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FTP_URI_USER);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FTP_URI_PATH);
			Assert.IsTrue(par.FtpUri.Port == FTP_URI_PORT);
			Assert.IsTrue(par.FtpUri.Scheme == FTP_URI_SCHEME);
			Assert.IsTrue(par.SqlServerConnectionString == SQL_SERVER_CONNECTION_STRING);
			Assert.IsTrue(par.MongoServerString == MONGO_SERVER_CONNECTION_STRING);
		}

		[Test]
		public void FilesShortTests()
		{
			var newDownloadPath = "NEW_DOWNLOAD_PATH";
			var newSuperFile = "NEW_SUPER_FILE";

			var par = new TestParametersClass(new[]
			{
				$"--d={newDownloadPath}",
				$"--sf={newSuperFile}"
			});

			Assert.IsFalse(par.DownloadPath == DOWNLOAD_PATH);
			Assert.IsTrue(par.DownloadPath == newDownloadPath);

			Assert.IsFalse(par.SuperFileToImport == SUPER_FILE_TO_IMPORT);
			Assert.IsTrue(par.SuperFileToImport == newSuperFile);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FTP_URI_HOST);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FTP_URI_USER);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FTP_URI_PATH);
			Assert.IsTrue(par.FtpUri.Port == FTP_URI_PORT);
			Assert.IsTrue(par.FtpUri.Scheme == FTP_URI_SCHEME);
			Assert.IsTrue(par.SqlServerConnectionString == SQL_SERVER_CONNECTION_STRING);
			Assert.IsTrue(par.MongoServerString == MONGO_SERVER_CONNECTION_STRING);
		}

		[Test]
		public void ParameterLessConstructorTest()
		{
			var par = new TestParametersClass();
			Assert.IsTrue(par.MongoServerString == "TEST_MONGO_CONNECTION_STRING_APP.CONFIG");
		}

		[Test]
		public void RadioShackTest()
		{
			var par = new TestParametersClass(new[] {"import2"});

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
		}

		[Test]
		public void SsisTest()
		{
			var par = new TestParametersClass(new[] {"import1"});

			Assert.IsTrue(par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
		}

		[Test]
		public void SweepTest()
		{
			var par = new TestParametersClass(new[] {"import3"});

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
		}
	}
}