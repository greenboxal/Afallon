using System;
using System.Reflection;
using System.Resources;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyCompany(Consts.Company)]
[assembly: AssemblyProduct(Consts.Product)]
[assembly: AssemblyCopyright(Consts.Copyright)]
[assembly: AssemblyVersion(Consts.FxVersion)]
[assembly: AssemblyFileVersion(Consts.FxFileVersion)]
[assembly: SatelliteContractVersion(Consts.Version)]
[assembly: AssemblyInformationalVersion(Consts.Version)]

[assembly: AssemblyDefaultAlias("PresentationFramework.dll")]
[assembly: AssemblyDescription("PresentationFramework.dll")]
[assembly: AssemblyTitle("PresentationFramework.dll")]

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]

[assembly: ComVisible(false)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]

[assembly: Dependency("mscorlib,", LoadHint.Always)]
[assembly: Dependency("System,", LoadHint.Always)]
[assembly: Dependency("WindowsBase,", LoadHint.Always)]
[assembly: Dependency("PresentationCore,", LoadHint.Always)]

[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml", "x")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/xps/2005/06", "metro")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "wpf")]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "av")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06/documentstructure", "System.Windows.Documents.DocumentStructures")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Controls")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Documents")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Shapes")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Navigation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Data")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Controls.Primitives")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Media.Animation")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/netfx/2007/xaml/presentation", "System.Windows.Input")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml", "System.Windows.Markup")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/xps/2005/06", "System.Windows.Controls")]
[assembly: ThemeInfo(ResourceDictionaryLocation.ExternalAssembly, ResourceDictionaryLocation.None)]
 