using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAutoEasyUI
{
    public class ProjectHelper
    {
        public void CreateDLLProject(string rootPath, string dllName, string rootNameSpace, string guid, List<string> csList, List<ReferInfo> referList)
        {
            int index = 0;
            StringBuilder csContent = new StringBuilder();
            foreach (var item in csList)
            {
                if (index == 0)
                {
                    csContent.AppendFormat("<Compile Include=\"{0}\" />", item);
                }
                else
                {
                    csContent.AppendFormat("\r\n  <Compile Include=\"{0}\" />", item);
                }

                index++;
            }

            index = 0;
            StringBuilder referContent = new StringBuilder();
            foreach (var item in referList)
            {
                if (index == 0)
                {
                    referContent.AppendFormat(@"<ItemGroup>
    <ProjectReference Include=""{0}"">
      <Project>{{{1}}}</Project>
      <Name>{2}</Name>
    </ProjectReference>
  </ItemGroup>", item.Path, item.Guid, item.Name);
                }
                else
                {
                    referContent.AppendFormat(@"  <ItemGroup>
    <ProjectReference Include=""{0}"">
      <Project>{{{1}}}</Project>
      <Name>{2}</Name>
    </ProjectReference>
  </ItemGroup>", item.Path, item.Guid, item.Name);
                }

                index++;
            }

            StringBuilder fileContent = new StringBuilder();
            fileContent.AppendFormat(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{{{0}}}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{1}</RootNamespace>
    <AssemblyName>{2}</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""System"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Xml"" />
  </ItemGroup>
  <ItemGroup>
    {3}
  </ItemGroup>
  {4}
  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>", guid, rootNameSpace, dllName, csContent.ToString(), referContent.ToString());

            // 写入文件csproj
            File.WriteAllText(Path.Combine(rootPath, rootNameSpace + ".csproj"), fileContent.ToString(), Encoding.UTF8);
        }

        public void CreateWebSiteProject(string rootPath, string dllName, string rootNameSpace, string guid, List<string> copyFileList, List<string> csList, List<string> aspxFileList, List<ReferInfo> referList)
        {
            int index = 0;
            StringBuilder referContent = new StringBuilder();
            if (referList != null)
            {
                referContent.Append("<ItemGroup>");
                foreach (var item in referList)
                {
                    if (index == 0)
                    {
                        referContent.AppendFormat(@"<ProjectReference Include=""{0}"">
      <Project>{{{1}}}</Project>
      <Name>{2}</Name>
    </ProjectReference>", item.Path, item.Guid, item.Name);
                    }
                    else
                    {
                        referContent.AppendFormat(@"\r\n  <ProjectReference Include=""{0}"">
      <Project>{{{1}}}</Project>
      <Name>{2}</Name>
    </ProjectReference>", item.Path, item.Guid, item.Name);
                    }

                    index++;
                }

                referContent.Append("</ItemGroup>");
            }

            index = 0;
            StringBuilder copyFileContent = new StringBuilder();
            if (copyFileList != null)
            {
                foreach (var item in copyFileList)
                {
                    if (index == 0)
                    {
                        copyFileContent.AppendFormat("<Content Include=\"{0}\" />", item);
                    }
                    else
                    {
                        copyFileContent.AppendFormat("\r\n  <Content Include=\"{0}\" />", item);
                    }

                    index++;
                }
            }

            StringBuilder csFileContent = new StringBuilder();
            if (aspxFileList != null)
            {
                foreach (var item in aspxFileList)
                {
                    csFileContent.AppendFormat(@"<Compile Include=""{0}.cs"">
      <DependentUpon>{0}</DependentUpon>
      <SubType>ASPXCodeBehind</SubType>
    </Compile>
    <Compile Include=""{0}.designer.cs"">
      <DependentUpon>{0}</DependentUpon>
    </Compile>", item);
                }
            }

            if (csList != null)
            {
                foreach (var item in csList)
                {
                    csFileContent.AppendFormat("<Compile Include=\"{0}\" />", item);
                }
            }

            // {349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}
            // 参考：http://blog.163.com/jeson_lwj/blog/static/13576108320141262143383/  ProjectTypeGuid
            StringBuilder fileContent = new StringBuilder();
            fileContent.AppendFormat(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProductVersion>
    </ProductVersion>
    <SchemaVersion>2.0</SchemaVersion>
    <ProjectGuid>{{{0}}}</ProjectGuid>
    <ProjectTypeGuids>{{349c5851-65df-11da-9384-00065b846f21}};{{fae04ec0-301f-11d3-bf4b-00c04f79efbc}}</ProjectTypeGuids>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>{1}</RootNamespace>
    <AssemblyName>{2}</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <UseIISExpress>true</UseIISExpress>
    <IISExpressSSLPort />
    <IISExpressAnonymousAuthentication />
    <IISExpressWindowsAuthentication />
    <IISExpressUseClassicPipelineMode />
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include=""Microsoft.CSharp"" />
    <Reference Include=""System.Web.DynamicData"" />
    <Reference Include=""System.Web.Entity"" />
    <Reference Include=""System.Web.ApplicationServices"" />
    <Reference Include=""System.ComponentModel.DataAnnotations"" />
    <Reference Include=""System"" />
    <Reference Include=""System.Data"" />
    <Reference Include=""System.Core"" />
    <Reference Include=""System.Data.DataSetExtensions"" />
    <Reference Include=""System.Web.Extensions"" />
    <Reference Include=""System.Xml.Linq"" />
    <Reference Include=""System.Drawing"" />
    <Reference Include=""System.Web"" />
    <Reference Include=""System.Xml"" />
    <Reference Include=""System.Configuration"" />
    <Reference Include=""System.Web.Services"" />
    <Reference Include=""System.EnterpriseServices"" />
  </ItemGroup>
  <ItemGroup>
    {3}
    <Content Include=""Web.config"" />
  </ItemGroup>
  <ItemGroup>
    {4}
  </ItemGroup>
    {5}
  <PropertyGroup>
    <VisualStudioVersion Condition=""'$(VisualStudioVersion)' == ''"">10.0</VisualStudioVersion>
    <VSToolsPath Condition=""'$(VSToolsPath)' == ''"">$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v$(VisualStudioVersion)</VSToolsPath>
  </PropertyGroup>
  <Import Project=""$(MSBuildBinPath)\Microsoft.CSharp.targets"" />
  <Import Project=""$(VSToolsPath)\WebApplications\Microsoft.WebApplication.targets"" Condition=""'$(VSToolsPath)' != ''"" />
  <Import Project=""$(MSBuildExtensionsPath32)\Microsoft\VisualStudio\v10.0\WebApplications\Microsoft.WebApplication.targets"" Condition=""false"" />
  <ProjectExtensions>
    <VisualStudio>
      <FlavorProperties GUID=""{{349c5851-65df-11da-9384-00065b846f21}}"">
        <WebProjectProperties>
          <UseIIS>True</UseIIS>
          <AutoAssignPort>True</AutoAssignPort>
          <DevelopmentServerPort>0</DevelopmentServerPort>
          <DevelopmentServerVPath>/</DevelopmentServerVPath>
          <IISUrl>http://localhost:35670/</IISUrl>
          <NTLMAuthentication>False</NTLMAuthentication>
          <UseCustomServer>False</UseCustomServer>
          <CustomServerUrl>
          </CustomServerUrl>
          <SaveServerSettingsInUserFile>False</SaveServerSettingsInUserFile>
        </WebProjectProperties>
      </FlavorProperties>
    </VisualStudio>
  </ProjectExtensions>
</Project>", guid, rootNameSpace, dllName, copyFileContent.ToString(), csFileContent.ToString(), referContent.ToString());

            // 写入文件csproj
            File.WriteAllText(Path.Combine(rootPath, rootNameSpace + ".csproj"), fileContent.ToString(), Encoding.UTF8);
        }
    }

    public class ReferInfo
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Guid { get; set; }
    }
}
