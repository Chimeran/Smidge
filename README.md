![Smidge](assets/logosmall.png?raw=true) Smidge
======

A lightweight **ASP.NET Core 1.0** library for _runtime_ CSS and JavaScript file management, minification, combination & compression.
Ported to ASP.NET Core 1.0 by Casper Spanggaard from Smidge for ASP.Net 5 by Shannon Deminick. See https://github.com/Shazwazza/Smidge.

## Install

_Builds using .NET Core command-line (CLI) tools version 1.0.0-preview1-002702 and ASP.NET Core 1.0 RC2

Clone the Git repository. Then:

```
$ cd Smidge\src\Smidge.Web
$ dotnet restore
$ dotnet run
```

Open http://localhost:5000/ in a browser.

__[See Installation](https://github.com/Shazwazza/Smidge/wiki/installation) of the original Smidge for ASP.NET 5 for full configuration details__ except install from source, use:

```
@inject Smidge.SmidgeHelper Smidge
@addTagHelper *, Smidge
```

(without quotes), and register a few services used by Smidge for ASP.NET Core 1.0:

```
        public void ConfigureServices(IServiceCollection services)
        {
			...
			
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            services.AddSmidge(_config)...
			
			...
		}
```

## Current status

```
Builds using the above specified versions of .NET Core 1.0 and ASP.NET Core 1.0.
Later versions may or may not build or run correctly.
Demo application runs.
Tests are not ported.
```

##Usage

### Pre-defined bundles

Define your bundles during startup:

```csharp
services.AddSmidge()
    .Configure<Bundles>(bundles =>
    {
        //Defining using JavaScriptFile's or CssFile's:

        bundles.Create("test-bundle-1", //bundle name
            new JavaScriptFile("~/Js/Bundle1/a1.js"),
            new JavaScriptFile("~/Js/Bundle1/a2.js"));

        //Or defining using file/folder paths:

        bundles.Create("test-bundle-2", WebFileType.Js, 
            "~/Js/Bundle2", "~/Js/OtherFolder*js");
    });
```

If you don't want to create named bundles and just want to declare dependencies individually inside your Views, you can do that too! You can create bundles (named or unnamed) during runtime ... no problem.

__[See Declarations](https://github.com/Shazwazza/Smidge/wiki/Declarations) for full declaration/usage details__

### Rendering

The easiest way to render bundles is simply by the bundle name:

```html
<script src="my-awesome-js-bundle" type="text/javascript"></script>
<link rel="stylesheet" href="my-cool-css-bundle"/>
```
    
This uses Smidge's custom tag helpers to check if the source is a bundle reference and will output the correct bundle URL. You can combine this with environment variables for debug/non-debug modes. Alternatively, you can also use Razor to do the rendering.

__[See Rendering](https://github.com/Shazwazza/Smidge/wiki/Rendering) for full rendering & debug mode details__

### Custom pre-processing pipeline

It's easy to customize how your files are processed. This can be done at a global/default level, at the bundle level or at an individual file level.

__[See Rendering](https://github.com/Shazwazza/Smidge/wiki/Custom-pre-processing) for information about customizing the pre-process pipeline__

### URLs

There's a couple of methods you can use retrieve the URLs that Smidge will generate when rendering the `<link>` or `<script>` html tags. This might be handy in case you need to load in these assets manually (i.e. lazy load scripts, etc...):

```csharp
Task<IEnumerable<string>> SmidgeHelper.GenerateJsUrlsAsync()
Task<IEnumerable<string>> SmidgeHelper.GenerateCssUrlsAsync()
```

__[See Asset URLs](https://github.com/Shazwazza/Smidge/wiki/Asset-Urls) for information about retrieving the debug and non-debug asset urls for your bundles__    

##Documentation

__[All of the documentation live here](https://github.com/Shazwazza/Smidge/wiki)__

##Work in progress

I haven't had time to document all of the features and extensibility points just yet and some of them are not quite finished but all of the usages documented above work.

## Copyright & Licence

&copy; 2016 by Shannon Deminick
&copy; 2016 by Casper Spanggaard

This is free software and is licensed under the [MIT License](http://opensource.org/licenses/MIT)

Logo image <a href="http://www.freepik.com">Designed by Freepik</a>
