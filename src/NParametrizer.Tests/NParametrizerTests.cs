using NUnit.Framework;

namespace NParametrizer.Tests
{
	[TestFixture]
	public class NParametrizerTests
	{
		private readonly string SuperFileToImport = "TEST_SUPERFILE_FILE_NAME";
		private readonly string DownloadPath = "TEST_DIRECTORY_PATH";
		private readonly string FtpUriHost = "test.server.test";
		private readonly string FtpUriPassword = "test-password";
		private readonly string FtpUriPath = "/Test/Path";
		private readonly int FtpUriPort = 22;
		private readonly string FtpUriScheme = "sftp";
		private readonly string FtpUriUser = "test-user";
		private readonly string SqlServerConnectionString = "TEST_SQLSERVER_CONNECTION_STRING_APP.CONFIG";
		private readonly string MongoServerConnectionString = "TEST_MONGO_CONNECTION_STRING_APP.CONFIG";

		private string FtpUriFull
		{
			get
			{
				return string.Format("{0}://{1}:{2}@{3}:{4}{5}",
					FtpUriScheme,
					FtpUriUser,
					FtpUriPassword,
					FtpUriHost,
					FtpUriPort,
					FtpUriPath
					);
			}
		}

		[Test]
		public void EnumTest()
		{
			var par = new TestParametersClass(new []{"--ENM=Value1"});
			Assert.IsTrue(par.EnumValue.Equals(TestEnum.Value1));

			var par1 = new TestParametersClass(new[] { "--ENM=Value3" });
			Assert.IsTrue(par1.EnumValue.Equals(TestEnum.Value3));

			var par2 = new TestParametersClass(new[] { "--AA=something"});
			Assert.IsTrue(par2.EnumNullableValue.Equals(null));

			var par3 = new TestParametersClass(new[] { "--ENMN=Value3" });
			Assert.IsTrue(par3.EnumNullableValue.Equals(TestEnum.Value3));
		}

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
			Assert.IsTrue(par.FtpUri.Host == FtpUriHost);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FtpUriUser);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FtpUriPath);
			Assert.IsTrue(par.FtpUri.Port == FtpUriPort);
			Assert.IsTrue(par.FtpUri.Scheme == FtpUriScheme);
			Assert.IsTrue(par.DownloadPath == DownloadPath);
			Assert.IsTrue(par.SqlServerConnectionString == SqlServerConnectionString);
			Assert.IsTrue(par.MongoServerString == MongoServerConnectionString);
		}

		[Test]
		public void ConnectionStringsTest()
		{
			var newSql = "NEW_SQL_CONNECTION_STRING";
			var newMongo = "NEW_SSIS_CONNECTION_STRING";

			var par = new TestParametersClass(new[] {string.Format("--S={0}", newSql), string.Format("--M={0}", newMongo)});

			Assert.IsTrue(par.SqlServerConnectionString == newSql);
			Assert.IsTrue(par.MongoServerString == newMongo);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FtpUriHost);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FtpUriUser);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FtpUriPath);
			Assert.IsTrue(par.FtpUri.Port == FtpUriPort);
			Assert.IsTrue(par.FtpUri.Scheme == FtpUriScheme);
			Assert.IsTrue(par.SuperFileToImport == SuperFileToImport);
			Assert.IsTrue(par.DownloadPath == DownloadPath);
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
			Assert.IsTrue(par.FtpUri.Host == FtpUriHost);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FtpUriUser);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FtpUriPath);
			Assert.IsTrue(par.FtpUri.Port == FtpUriPort);
			Assert.IsTrue(par.FtpUri.Scheme == FtpUriScheme);
			Assert.IsTrue(par.SuperFileToImport == SuperFileToImport);
			Assert.IsTrue(par.DownloadPath == DownloadPath);
			Assert.IsTrue(par.SqlServerConnectionString == SqlServerConnectionString);
			Assert.IsTrue(par.MongoServerString == MongoServerConnectionString);
		}

		[Test]
		public void FilesLongChangePrefixTests()
		{
			var newDownloadPath = "NEW_DOWNLOAD_PATH";
			var newSuperFile = "NEW_SUPER_FILE";

			var par = new TestParametersClass(new[]
			{
				string.Format("--DownloadPath={0}", newDownloadPath),
				string.Format("--SuperFileToImport={0}", newSuperFile)
			});

			Assert.IsFalse(par.DownloadPath == DownloadPath);
			Assert.IsTrue(par.DownloadPath == newDownloadPath);

			Assert.IsFalse(par.SuperFileToImport == SuperFileToImport);
			Assert.IsTrue(par.SuperFileToImport == newSuperFile);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FtpUriHost);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FtpUriUser);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FtpUriPath);
			Assert.IsTrue(par.FtpUri.Port == FtpUriPort);
			Assert.IsTrue(par.FtpUri.Scheme == FtpUriScheme);
			Assert.IsTrue(par.SqlServerConnectionString == SqlServerConnectionString);
			Assert.IsTrue(par.MongoServerString == MongoServerConnectionString);
		}

		[Test]
		public void FilesLongTests()
		{
			var newDownloadPath = "NEW_DOWNLOAD_PATH";
			var newSuperFile = "NEW_SUPER_FILE";

			var par = new TestParametersClass(new[]
			{
				string.Format("--DownloadPath={0}", newDownloadPath),
				string.Format("--SuperFileToImport={0}", newSuperFile)
			});

			Assert.IsFalse(par.DownloadPath == DownloadPath);
			Assert.IsTrue(par.DownloadPath == newDownloadPath);

			Assert.IsFalse(par.SuperFileToImport == SuperFileToImport);
			Assert.IsTrue(par.SuperFileToImport == newSuperFile);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FtpUriHost);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FtpUriUser);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FtpUriPath);
			Assert.IsTrue(par.FtpUri.Port == FtpUriPort);
			Assert.IsTrue(par.FtpUri.Scheme == FtpUriScheme);
			Assert.IsTrue(par.SqlServerConnectionString == SqlServerConnectionString);
			Assert.IsTrue(par.MongoServerString == MongoServerConnectionString);
		}

		[Test]
		public void FilesShortTests()
		{
			var newDownloadPath = "NEW_DOWNLOAD_PATH";
			var newSuperFile = "NEW_SUPER_FILE";

			var par = new TestParametersClass(new[]
			{
				string.Format("--d={0}", newDownloadPath),
				string.Format("--sf={0}", newSuperFile)
			});

			Assert.IsFalse(par.DownloadPath == DownloadPath);
			Assert.IsTrue(par.DownloadPath == newDownloadPath);

			Assert.IsFalse(par.SuperFileToImport == SuperFileToImport);
			Assert.IsTrue(par.SuperFileToImport == newSuperFile);

			Assert.IsTrue(!par.Import1);
			Assert.IsTrue(!par.Import2);
			Assert.IsTrue(!par.Import3);
			Assert.IsTrue(!par.PerformFtpDownload);
			Assert.IsTrue(par.FtpUri.ToString() == FtpUriFull);
			Assert.IsTrue(par.FtpUri.Host == FtpUriHost);
			Assert.IsTrue(par.FtpUri.Credentials.UserName == FtpUriUser);
			Assert.IsTrue(par.FtpUri.AbsolutePath == FtpUriPath);
			Assert.IsTrue(par.FtpUri.Port == FtpUriPort);
			Assert.IsTrue(par.FtpUri.Scheme == FtpUriScheme);
			Assert.IsTrue(par.SqlServerConnectionString == SqlServerConnectionString);
			Assert.IsTrue(par.MongoServerString == MongoServerConnectionString);
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
			Assert.IsTrue(par.SuperFileToImport == SuperFileToImport);
			Assert.IsTrue(par.DownloadPath == DownloadPath);
			Assert.IsTrue(par.SqlServerConnectionString == SqlServerConnectionString);
			Assert.IsTrue(par.MongoServerString == MongoServerConnectionString);
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
		public void SSISTest()
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