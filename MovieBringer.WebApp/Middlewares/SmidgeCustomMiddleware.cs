using Smidge;
using Smidge.Models;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MovieBringer.WebApp.Middlewares
{
    public static class SmidgeCustomMiddleware
    {
        public static IApplicationBuilder UseMySmidge(this IApplicationBuilder app)
        {
            //layouts bundle
            app.UseSmidge(bundle =>
            {                
                bundle.CreateJs("my-js-bundle", "~/js/Custom/sweetAlertCustom.js", "~/Resource/MainHotFlix/js/main.js", "~/Resource/MainHotFlix/js/wNumb.js");

                bundle.CreateCss("my-css-bundle","~/Resource/MainHotFlix/css/main.css",
                    "~/Resource/MainHotFlix/css/magnific-popup.css",
                    "~/Resource/MainHotFlix/css/plyr.css",
                    "~/Resource/MainHotFlix/css/photoswipe.css",
                    "~/Resource/MainHotFlix/css/default-skin.css");
                //bundle.CreateCss("my-css-admin-bundle", "~/Resource/AdminHotFlix/css/select2.min.css");


                //views js bundle
                var viewsJsBundle = CreateViewsJsBundle();
                foreach (var createJsModel in viewsJsBundle)
                {
                    bundle.CreateJs(createJsModel.BundleName, createJsModel.JsPath);
                }
            });

            return app;
        }

        static List<CreateJsModel> CreateViewsJsBundle()
        {
            List<CreateJsModel> myList = new List<CreateJsModel>();

            string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "js", "Views");
            var viewDirectories = Directory.GetDirectories(basePath, "*", SearchOption.AllDirectories);

            foreach (var viewDirectory in viewDirectories)
            {
                string[] jsFiles = Directory.GetFiles(viewDirectory, "*.js");

                foreach (var jsFile in jsFiles)
                {
                    string jsFileName = Path.GetFileName(jsFile);
                    string jsFileNameBundle= Path.GetFileNameWithoutExtension(jsFileName).ToLower();
                    string bundleName = $"{Path.GetFileName(viewDirectory).ToLower()}-{jsFileNameBundle}-bundle";
                    string virtualPath = $"~/js/Views/{Path.GetFileName(viewDirectory)}/{jsFileName}";

                    //Console.WriteLine($"Bundle: {bundleName}, JS File: {virtualPath}");
                    myList.Add(new CreateJsModel { BundleName = bundleName, JsPath = virtualPath });
                  
                }
            }

            return myList;
        }

        private class CreateJsModel
        {
            public string BundleName { get; set; }
            public string JsPath { get; set; }
        }
    }
}
