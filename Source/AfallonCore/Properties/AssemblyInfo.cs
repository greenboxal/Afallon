using System;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: AssemblyCompany(Consts.Company)]
[assembly: AssemblyProduct(Consts.Product)]
[assembly: AssemblyCopyright(Consts.Copyright)]
[assembly: AssemblyVersion(Consts.FxVersion)]
[assembly: AssemblyFileVersion(Consts.FxFileVersion)]
[assembly: SatelliteContractVersion(Consts.Version)]
[assembly: AssemblyInformationalVersion(Consts.Version)]

[assembly: AssemblyDefaultAlias("PresentationCore.dll")]
[assembly: AssemblyDescription("PresentationCore.dll")]
[assembly: AssemblyTitle("PresentationCore.dll")]

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]

[assembly: ComVisible(false)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]

[assembly: Dependency("WindowsBase,", LoadHint.Always)]
[assembly: Dependency("System,", LoadHint.Always)]

[assembly: InternalsVisibleTo("PresentationFramework, PublicKey=00240000048000009400000006020000002400005253413100040000010001004983cdcb8412f5dd7fbf89892d25b6de102b16ef7f4ee89bc5fd3bf4eb2b82df74622f124345eaa550bb1aad6080821f22cc436a1d1bcff1bf86981f9afe9bc442519086b3c00889db4c9896d7927888238ba3c2111c0f269c95c49bfed725dcf1758e687980332586bfc591675d63e256a4221011eef296447b7c58578122bc")]

[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml", "x")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/xps/2005/06", "xps")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "av")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "wpf")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.Effects")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Ink")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.Imaging")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.TextFormatting")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.Effects")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.Imaging")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.Imaging")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.TextFormatting")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "System.Windows.Markup")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/composite-font", "System.Windows.Media")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.Media3D")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.Media3D")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Automation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.Effects")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.Media3D")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Automation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.TextFormatting")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Automation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Ink")]
 