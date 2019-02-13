using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

[assembly: PreApplicationStartMethod(typeof(E_Shop_Engine.Website.App_Start.PluginConfig), "RegisterPlugins")]
namespace E_Shop_Engine.Website.App_Start
{
    public static class PluginConfig
    {
        static PluginConfig()
        {
            PluginFolder = new DirectoryInfo(Path.Combine(HostingEnvironment.MapPath("/"), @"App_Data\Plugins"));
        }

        /// <summary>
        /// The source plugin folder from which to shadow copy from
        /// </summary>
        /// <remarks>
        /// This folder can contain sub folderst to organize plugin types
        /// </remarks>
        private static readonly DirectoryInfo PluginFolder;

        public static void RegisterPlugins()
        {
            // Now, we need to tell the BuildManager that our plugin DLLs exists and to reference them.
            // There are different Assembly Load Contexts that we need to take into account which 
            // are defined in this article here:
            // http://blogs.msdn.com/b/suzcook/archive/2003/05/29/57143.aspx

            // * This will put the plugin assemblies in the 'Load' context
            // This works but requires a 'probing' folder be defined in the web.config
            //foreach (Assembly a in
            //    ShadowCopyFolder
            //    .GetFiles("*.dll", SearchOption.AllDirectories)
            //    .Select(x => AssemblyName.GetAssemblyName(x.FullName))
            //    .Select(x => Assembly.Load(x.FullName)))
            //{
            //    BuildManager.AddReferencedAssembly(a);
            //}

            // * This will put the plugin assemblies in the 'LoadFrom' context
            // This works but requires a 'probing' folder be defined in the web.config
            // This is the slowest and most error prone version of the Load contexts.            
            //foreach (Assembly a in
            //    PluginFolder
            //    .GetFiles("*.dll", SearchOption.AllDirectories)
            //    .Select(plug => Assembly.LoadFrom(plug.FullName)))
            //{
            //    BuildManager.AddReferencedAssembly(a);
            //}

            // * This will put the plugin assemblies in the 'Neither' context ( i think )
            // This nearly works but fails during view compilation.
            // This DOES work for resolving controllers but during view compilation which is done with the RazorViewEngine, 
            // the CodeDom building doesn't reference the plugin assemblies directly.
            foreach (Assembly a in
                PluginFolder
                .GetFiles("*.dll", SearchOption.AllDirectories)
                .Select(plug => Assembly.Load(File.ReadAllBytes(plug.FullName))))
            {
                BuildManager.AddReferencedAssembly(a);
            }

        }
    }
}