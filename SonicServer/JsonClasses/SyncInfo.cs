using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonicServer.JsonClasses
{
    public struct SyncInfo
    {
        public struct MarketingInfo
        {
            public string ActiveCampaign { get; set; }
            public string ActiveCatalog { get; set; }
            public List<string> Campaigns { get; set; }
            public List<string> Catalogs { get; set; }
            public List<string> MediaFiles { get; set; }
        }

        public struct SoftwareInfo
        {
            public string ActiveVersion { get; set; }
            public CurrentConfigInfo CurrentConfig { get; set; }
            public object FeaturesConfig { get; set; }
            public List<string> InstallSchedule { get; set; }
            public object InstalledManifest { get; set; }
            public object PaymentConfig { get; set; }
            public SetupConfigInfo SetupConfig { get; set; }
            public object TuningConfig { get; set; }
            public List<string> VersionSchedule { get; set; }
        }

        public struct CurrentConfigInfo
        {
            public AppInfo App { get; set; }
            public CarDetectInfo CarDetect { get; set; }
            public ExperienceInfo Experience { get; set; }
            public FileCheckInfo FileCheck { get; set; }
            public FileTransferInfo FileTransfer { get; set; }
            public PaymentInfo Payment { get; set; }
            public SensorsInfo Sensors { get; set; }
            public SetupInfo Setup { get; set; }
        }

        public struct AppInfo
        {
            public bool AbleToChangeComputerSettings { get; set; }
            public string AdminLogin { get; set; }
            public string DnsServers { get; set; }
            public bool EnableDeprecatedTraces { get; set; }
            public bool EnableMediaShowingInEmpty { get; set; }
            public bool EnableWindowsTimeResync { get; set; }
            public int GlobalLogLevel { get; set; }
            public int GzipChunkDelayMilliseconds { get; set; }
            public int GzipNumConsecutiveInputChunks { get; set; }
            public bool KioskMode { get; set; }
            public long LowMemoryThresholdBytes { get; set; }
            public int NetworkSettingsRestartTries { get; set; }
            public int NumDaysLogsToKeep { get; set; }
            public int ReportGenerationTimeout { get; set; }
            public string RootFolder { get; set; }
            public bool RunHeadless { get; set; }
            public bool ShowConsole { get; set; }
            public bool ShowFrameRate { get; set; }
            public int StallDownTimeoutSeconds { get; set; }
            public int StallOrientation { get; set; }
            public int StallReconnectTimeoutSeconds { get; set; }
            public string StallType { get; set; }
            public int TcpIpAdapterOperationTimeoutSeconds { get; set; }
            public int WindowsTimeResyncTimerSeconds { get; set; }
        }

        public struct CarDetectInfo
        {
            public bool CarDetectEnabled { get; set; }
            public int SonarCarRangeThreshold { get; set; }
            public int SonarCarTimeThresholdMilliseconds { get; set; }
            public string SonarDetectionAlgorithm { get; set; }
            public int SonarNoCarRangeThreshold { get; set; }
            public int SonarNoCarTimeThresholdMilliseconds { get; set; }
        }

        public struct ExperienceInfo
        {
            public int AdminIdleTimerSeconds { get; set; }
            public int CarhopAssistIdleTimerSeconds { get; set; }
            public int CarhopRetrieveTicketTimeoutSeconds { get; set; }
            public int DeliveryNotificationDelaySeconds { get; set; }
            public int DeliveryTimeoutSeconds { get; set; }
            public int EatTimeoutSeconds { get; set; }
            public int LoginTimeoutSeconds { get; set; }
            public int OrderConfirmationWaitSeconds { get; set; }
            public int PreOrderTimeoutSeconds { get; set; }
            public int RedButtonTimeoutSeconds { get; set; }
            public int RetailPaymentModalTimeoutSeconds { get; set; }
            public int RetailPaymentResultsModalTimeoutSeconds { get; set; }
            public bool TippingEnabled { get; set; }
        }

        public struct FileCheckInfo
        {
            public string FileCheckFullPath { get; set; }
            public long MaxFileSizeBytes { get; set; }
            public int PollTimeSeconds { get; set; }
            public int ThresholdTimeSeconds { get; set; }
        }

        public struct FileTransferInfo
        {
            public int BlockSizeBytes { get; set; }
            public int BlocksInFlight { get; set; }
        }

        public struct PaymentInfo
        {
            public string ConnectPaymentProcessorIP { get; set; }
            public int ConnectPaymentProcessorPort { get; set; }
            public int ConnectPaymentProcessorReconnectTimeoutSeconds { get; set; }
            public string PaymentProcessorExeDir { get; set; }
            public string PaymentProcessorExeName { get; set; }
            public int PaymentProcessorHeartbeatTimeoutSeconds { get; set; }
            public int PaymentProcessorHeartbeatTimerSeconds { get; set; }
            public int PaymentProcessorTcpKeepaliveIdleTimeSeconds { get; set; }
            public int PaymentProcessorTcpKeepaliveRetryTimeSeconds { get; set; }
            public int PaymentResolutionResendTimeoutSeconds { get; set; }
            public int RetailMaxClaimAttempts { get; set; }
            public int TicketClaimTimeoutSeconds { get; set; }
        }

        public struct SensorsInfo
        {
            public bool RedButtonBadSensorCheckEnabled { get; set; }
            public int RedButtonBadTimeDelaySeconds { get; set; }
            public int RedButtonEventLimit { get; set; }
            public int RedButtonTimeLimitSeconds { get; set; }
            public bool ScreenBadSensorCheckEnabled { get; set; }
            public int ScreenBadTimeDelaySeconds { get; set; }
            public int ScreenEventLimit { get; set; }
            public int ScreenTimeLimitSeconds { get; set; }
            public bool SonarBadSensorCheckEnabled { get; set; }
            public int SonarBadTimeDelaySeconds { get; set; }
            public int SonarEventLimit { get; set; }
            public int SonarTimeLimitSeconds { get; set; }
        }

        public struct SetupInfo
        {
            public string NetworkType { get; set; }
            public string RedButtonProvider { get; set; }
            public int StallNumber { get; set; }
            public string StoreIP { get; set; }
            public int StoreNumber { get; set; }
            public int StorePort { get; set; }
            public string TimeZone { get; set; }
        }

        public struct SetupConfigInfo
        {
            public AppSetupInfo App { get; set; }
            public SetupInfo Setup { get; set; }
        }

        public struct AppSetupInfo
        {
            public int StallOrientation { get; set; }
            public string StallType { get; set; }
        }


        public MarketingInfo Marketing { get; set; }
        public SoftwareInfo Software { get; set; }
        public object Streams { get; set; }
    }

}
