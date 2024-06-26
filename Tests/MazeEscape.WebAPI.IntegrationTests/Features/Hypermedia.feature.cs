﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace MazeEscape.WebAPI.IntegrationTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Hypermedia")]
    public partial class HypermediaFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
#line 1 "Hypermedia.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Hypermedia", "Test links and actions returned from endpoints", ProgrammingLanguage.CSharp, featureTags);
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get mazes root")]
        public void GetMazesRoot()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get mazes root", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 7
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
 testRunner.When("I make a GET request to:/mazes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 9
 testRunner.Then("the status code is:OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table1.AddRow(new string[] {
                            "create-maze-from-preset",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"preset\",\"preset\":{\"presetName\":\"{presetName}\"}}"});
                table1.AddRow(new string[] {
                            "create-maze-from-text",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"custom\",\"custom\":{\"mazeText\":\"{mazeText}\"}}"});
                table1.AddRow(new string[] {
                            "create-random-maze",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"random\",\"random\":{\"width\":\"{width}\",\"height\":\"{height}\"}}"});
#line 10
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table1, "And ");
#line hidden
                TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table2.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
                table2.AddRow(new string[] {
                            "get-mazes-presets-list",
                            "/mazes/presets",
                            "GET",
                            ""});
#line 15
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table2, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Get mazes presets")]
        public void GetMazesPresets()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get mazes presets", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 20
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 21
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 22
 testRunner.When("I make a GET request to:/mazes/presets", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 23
 testRunner.Then("the status code is:OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table3.AddRow(new string[] {
                            "create-maze-from-preset",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"preset\",\"preset\":{\"presetName\":\"{presetName}\"}}"});
#line 24
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table3, "And ");
#line hidden
                TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table4.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
#line 27
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table4, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Error Scenario: Create maze from empty preset - Returns root hypermedia")]
        public void ErrorScenarioCreateMazeFromEmptyPreset_ReturnsRootHypermedia()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Error Scenario: Create maze from empty preset - Returns root hypermedia", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 31
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 32
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 33
 testRunner.When("I make a POST request to:/mazes?createMode=preset with body:{\"preset\": {\"presetNa" +
                        "me\": \"\"}}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 34
 testRunner.Then("the status code is:BadRequest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table5.AddRow(new string[] {
                            "create-maze-from-preset",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"preset\",\"preset\":{\"presetName\":\"{presetName}\"}}"});
                table5.AddRow(new string[] {
                            "create-maze-from-text",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"custom\",\"custom\":{\"mazeText\":\"{mazeText}\"}}"});
                table5.AddRow(new string[] {
                            "create-random-maze",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"random\",\"random\":{\"width\":\"{width}\",\"height\":\"{height}\"}}"});
#line 35
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table5, "And ");
#line hidden
                TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table6.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
                table6.AddRow(new string[] {
                            "get-mazes-presets-list",
                            "/mazes/presets",
                            "GET",
                            ""});
#line 40
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table6, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Error Scenario: Create maze from a non-existent preset - Returns root hypermedia")]
        public void ErrorScenarioCreateMazeFromANon_ExistentPreset_ReturnsRootHypermedia()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Error Scenario: Create maze from a non-existent preset - Returns root hypermedia", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 45
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 46
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 47
 testRunner.When("I make a POST request to:/mazes?createMode=preset with body:{\"preset\": {\"presetNa" +
                        "me\": \"doesntExist\"}}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 48
 testRunner.Then("the status code is:NotFound", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table7.AddRow(new string[] {
                            "create-maze-from-preset",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"preset\",\"preset\":{\"presetName\":\"{presetName}\"}}"});
                table7.AddRow(new string[] {
                            "create-maze-from-text",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"custom\",\"custom\":{\"mazeText\":\"{mazeText}\"}}"});
                table7.AddRow(new string[] {
                            "create-random-maze",
                            "/mazes",
                            "POST",
                            "{\"createMode\":\"random\",\"random\":{\"width\":\"{width}\",\"height\":\"{height}\"}}"});
#line 49
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table7, "And ");
#line hidden
                TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table8.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
                table8.AddRow(new string[] {
                            "get-mazes-presets-list",
                            "/mazes/presets",
                            "GET",
                            ""});
#line 54
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table8, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Post mazes")]
        public void PostMazes()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post mazes", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 59
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 60
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 61
 testRunner.When("I make a POST request to:/mazes?createMode=preset with body:{\"preset\": {\"presetNa" +
                        "me\": \"spiral\"}}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 62
 testRunner.Then("the status code is:Created", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table9.AddRow(new string[] {
                            "post-player",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\"}"});
#line 63
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table9, "And ");
#line hidden
                TechTalk.SpecFlow.Table table10 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table10.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
#line 66
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table10, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Error Scenario: Get Player Info without mazeToken  - Still returns hypermedia")]
        public void ErrorScenarioGetPlayerInfoWithoutMazeToken_StillReturnsHypermedia()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Error Scenario: Get Player Info without mazeToken  - Still returns hypermedia", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 70
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 71
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 72
 testRunner.When("I make a POST request to:/mazes/player with body:{\"mazeToken\": \"\"}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 73
 testRunner.Then("the status code is:BadRequest", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table11 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table11.AddRow(new string[] {
                            "post-player",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\"}"});
                table11.AddRow(new string[] {
                            "player-turn-left",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\",\"playerMove\":\"turnLeft\"}"});
                table11.AddRow(new string[] {
                            "player-turn-right",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\",\"playerMove\":\"turnRight\"}"});
                table11.AddRow(new string[] {
                            "player-move-forward",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\",\"playerMove\":\"forward\"}"});
#line 74
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table11, "And ");
#line hidden
                TechTalk.SpecFlow.Table table12 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table12.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
#line 80
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table12, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Post Player")]
        public void PostPlayer()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Player", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 84
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 85
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 86
 testRunner.When("I make a POST request to:/mazes?createMode=preset with body:{\"preset\": {\"presetNa" +
                        "me\": \"spiral\"}}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 87
 testRunner.And("I save the mazeToken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 88
 testRunner.And("I make a POST request to:/mazes/player with saved mazeToken and body:{\"mazeToken\"" +
                        ":\"{mazeToken}\"}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 89
 testRunner.Then("the status code is:OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table13 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table13.AddRow(new string[] {
                            "post-player",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\"}"});
                table13.AddRow(new string[] {
                            "player-turn-left",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\",\"playerMove\":\"turnLeft\"}"});
                table13.AddRow(new string[] {
                            "player-turn-right",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\",\"playerMove\":\"turnRight\"}"});
                table13.AddRow(new string[] {
                            "player-move-forward",
                            "/mazes/player",
                            "POST",
                            "{\"mazeToken\":\"{mazeToken}\",\"playerMove\":\"forward\"}"});
#line 90
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table13, "And ");
#line hidden
                TechTalk.SpecFlow.Table table14 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table14.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
#line 96
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table14, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("No actions available after maze has been escaped")]
        public void NoActionsAvailableAfterMazeHasBeenEscaped()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No actions available after maze has been escaped", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 100
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((TagHelper.ContainsIgnoreTag(tagsOfScenario) || TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 101
 testRunner.Given("the MazeEscape client is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 102
 testRunner.When("I make a POST request to:/mazes with body:{\"createMode\":\"preset\", \"preset\": {\"pre" +
                        "setName\": \"minmaze\"}}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 103
 testRunner.And("I save the mazeToken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.And("I make a POST request to:/mazes/player with saved mazeToken and body:{\"mazeToken\"" +
                        ":\"{mazeToken}\"}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 105
 testRunner.And("I save the mazeToken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 106
 testRunner.And("I make a POST request to:/mazes/player with saved mazeToken and body:{\"mazeToken\"" +
                        ":\"{mazeToken}\",\"playerMove\":\"forward\"}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 107
 testRunner.And("I save the mazeToken", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 108
 testRunner.And("I make a POST request to:/mazes/player with saved mazeToken and body:{\"mazeToken\"" +
                        ":\"{mazeToken}\",\"playerMove\":\"forward\"}", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 109
 testRunner.Then("the response message contains:You escaped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 110
 testRunner.Then("the response message contains:You escaped", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
                TechTalk.SpecFlow.Table table15 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
#line 111
 testRunner.And("the response contains the following hypermedia array:actions with values:", ((string)(null)), table15, "And ");
#line hidden
                TechTalk.SpecFlow.Table table16 = new TechTalk.SpecFlow.Table(new string[] {
                            "description",
                            "href",
                            "method",
                            "body"});
                table16.AddRow(new string[] {
                            "get-mazes-root",
                            "/mazes",
                            "GET",
                            ""});
#line 113
 testRunner.And("the response contains the following hypermedia array:links with values:", ((string)(null)), table16, "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
