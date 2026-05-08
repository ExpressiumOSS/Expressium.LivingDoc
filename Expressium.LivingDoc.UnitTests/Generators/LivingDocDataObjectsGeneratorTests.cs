using Expressium.LivingDoc.Generators;
using Expressium.LivingDoc.Models;
using System;
using System.Collections.Generic;

namespace Expressium.LivingDoc.UnitTests.Generators
{
    public class LivingDocDataObjectsGeneratorTests
    {
        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatures()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Name = "Login",
                Tags = new List<string> { "@Login" }
            };
            livingDocProject.Features.Add(feature);

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatures();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines, Does.Contain("<!-- Data Feature -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Feature Section -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Feature Name -->"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarios()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature { Name = "Login" };
            var scenario = new LivingDocScenario { Name = "Successful User Login with Valid Credentials" };
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep { Keyword = "Given", Name = "I have logged in", Status = "Passed" });
            scenario.Examples.Add(example);
            feature.Scenarios.Add(scenario);
            livingDocProject.Features.Add(feature);

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarios();

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines, Does.Contain("<!-- Data Scenario -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Scenario Section -->"));
            Assert.That(listOfLines, Does.Contain("<!-- Data Scenario Steps -->"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioDescription()
        {
            var livingDocProject = new LivingDocProject();

            var scenario = new LivingDocScenario
            {
                Description = "Line One\nLine Two\nLine Three"
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioDescription(scenario);

            Assert.That(listOfLines, Is.Not.Null);
            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Description -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<ul class='scenario-description'>"));
            Assert.That(listOfLines[3], Is.EqualTo("<li>Line One</li>"));
            Assert.That(listOfLines[4], Is.EqualTo("<li>Line Two</li>"));
            Assert.That(listOfLines[5], Is.EqualTo("<li>Line Three</li>"));
            Assert.That(listOfLines[6], Is.EqualTo("</ul>"));
            Assert.That(listOfLines[7], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioDescription_ReturnsEmpty_WhenNullOrWhitespace()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());

            var scenario = new LivingDocScenario { Description = null };
            Assert.That(generator.GenerateDataScenarioDescription(scenario), Is.Empty);

            scenario.Description = "   ";
            Assert.That(generator.GenerateDataScenarioDescription(scenario), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeaturesTags()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Tags = new List<string> { "@tag1", "@tag2" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureTags(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div class='feature-tag-group'>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='feature-tag'>@tag1</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='feature-tag'>@tag2</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeaturesName()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Name = "Feature Name",
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureName(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='bi bi-dash-circle-fill color-skipped status-symbol'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='feature-keyword'>Feature: </span><span class='feature-name'>Feature Name</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureDescription()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Description = "Line One\nLine Two\nLine Three"
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureDescription(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(9));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Description -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<ul class='feature-description'>"));
            Assert.That(listOfLines[3], Is.EqualTo("<li>Line One</li>"));
            Assert.That(listOfLines[4], Is.EqualTo("<li>Line Two</li>"));
            Assert.That(listOfLines[5], Is.EqualTo("<li>Line Three</li>"));
            Assert.That(listOfLines[6], Is.EqualTo("</ul>"));
            Assert.That(listOfLines[7], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureDescription_ReturnsOnlyHr_WhenDescriptionIsNull()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var feature = new LivingDocFeature { Description = null };

            var listOfLines = generator.GenerateDataFeatureDescription(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(1));
            Assert.That(listOfLines[0], Is.EqualTo("<hr>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureBackground()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Background = new LivingDocBackground
                {
                    Steps = new List<LivingDocStep>
                    {
                        new LivingDocStep
                        {
                            Keyword = "Given",
                            Name = "I have logged in to the application"
                        }
                    }
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureBackground(feature);

            Assert.That(listOfLines.Count, Is.EqualTo(15));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Feature Background -->"));
            Assert.That(listOfLines[8], Is.EqualTo("<span class='step-keyword'>Given</span>"));
            Assert.That(listOfLines[9], Is.EqualTo("<span class='step-name'>I have logged in to the application</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureBackground_ReturnsEmpty_WhenNoBackground()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var feature = new LivingDocFeature { Background = null };

            Assert.That(generator.GenerateDataFeatureBackground(feature), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureBackground_ReturnsEmpty_WhenBackgroundHasNoSteps()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var feature = new LivingDocFeature { Background = new LivingDocBackground() };

            Assert.That(generator.GenerateDataFeatureBackground(feature), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataFeatureBackgroundSteps()
        {
            var livingDocProject = new LivingDocProject();

            var feature = new LivingDocFeature
            {
                Background = new LivingDocBackground
                {
                    Steps = new List<LivingDocStep>
                    {
                        new LivingDocStep
                        {
                            Keyword = "Given",
                            Name = "I have logged in to the application"
                        }
                    }
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataFeatureBackgroundSteps(feature.Background.Steps);

            Assert.That(listOfLines.Count, Is.EqualTo(10));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Background Steps -->"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='step-keyword'>Given</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span class='step-name'>I have logged in to the application</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleTags()
        {
            var livingDocProject = new LivingDocProject();

            var rule = new LivingDocRule
            {
                Tags = new List<string> { "@tag5", "@tag6" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataRuleTags(rule);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Rule Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div class='rule-tag-group'>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='rule-tag'>@tag5</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='rule-tag'>@tag6</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleName()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var rule = new LivingDocRule { Name = "Sale cannot happen without stock" };

            var listOfLines = generator.GenerateDataRuleName(rule);

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Rule Name -->"));
            Assert.That(listOfLines, Does.Contain("<span class='rule-keyword'>Rule: </span>"));
            Assert.That(listOfLines, Does.Contain("<span class='rule-name'>Sale cannot happen without stock</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleDescription_RendersDescription_WhenSet()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var rule = new LivingDocRule { Description = "Line One\nLine Two" };

            var listOfLines = generator.GenerateDataRuleDescription(rule);

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Rule Description -->"));
            Assert.That(listOfLines, Does.Contain("<li>Line One</li>"));
            Assert.That(listOfLines, Does.Contain("<li>Line Two</li>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataRuleDescription_ReturnsEmpty_WhenNullOrWhitespace()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());

            Assert.That(generator.GenerateDataRuleDescription(new LivingDocRule { Description = null }), Is.Empty);
            Assert.That(generator.GenerateDataRuleDescription(new LivingDocRule { Description = "   " }), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioRule()
        {
            var livingDocProject = new LivingDocProject();

            var rule = new LivingDocRule
            {
                Name = "Orders",
                Tags = new List<string> { "@tag5", "@tag6" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioRule(rule, null);

            Assert.That(listOfLines.Count, Is.EqualTo(14));
            Assert.That(listOfLines[7], Is.EqualTo("<!-- Data Rule Name -->"));
            Assert.That(listOfLines[9], Does.Contain("Rule:"));
            Assert.That(listOfLines[10], Does.Contain("Orders"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioRule_Replica_WhenSameRuleId()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var rule = new LivingDocRule { Id = "rule-001", Name = "Orders" };

            var listOfLines = generator.GenerateDataScenarioRule(rule, "rule-001");

            Assert.That(listOfLines, Does.Contain("<div class='section' data-rule-replica>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioTags()
        {
            var livingDocProject = new LivingDocProject();

            var scenario = new LivingDocScenario
            {
                Tags = new List<string> { "@tag3", "@tag4" }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines.Count, Is.EqualTo(5));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Tags -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div class='scenario-tag-group'>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='scenario-tag'>@tag3</span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='scenario-tag'>@tag4</span>"));
            Assert.That(listOfLines[4], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioTags_WithHealth_RendersHealthTag()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var scenario = new LivingDocScenario
            {
                Tags = new List<string> { "@smoke" },
                Health = LivingDocHealths.Flaky.ToString()
            };

            var listOfLines = generator.GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines, Does.Contain("<span class='scenario-tag'>@smoke</span>"));
            Assert.That(listOfLines, Has.Some.Contains("scenario-tag-health"));
            Assert.That(listOfLines, Has.Some.Contains("color-flaky"));
            Assert.That(listOfLines, Has.Some.Contains("@Flaky"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioTags_WithoutHealth_DoesNotRenderHealthTag()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var scenario = new LivingDocScenario { Tags = new List<string> { "@smoke" } };

            var listOfLines = generator.GenerateDataScenarioTags(scenario);

            Assert.That(listOfLines, Has.None.Contains("scenario-tag-health"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName()
        {
            var livingDocProject = new LivingDocProject();

            var scenario = new LivingDocScenario
            {
                Name = "Scenario Name"
            };

            var example = new LivingDocExample
            {
                Duration = new TimeSpan(0, 0, 0, 1, 500)
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioName(scenario, example, "5");

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Name -->"));
            Assert.That(listOfLines[1], Is.EqualTo("<div>"));
            Assert.That(listOfLines[2], Is.EqualTo("<span class='bi bi-dash-circle-fill color-skipped status-symbol'></span>"));
            Assert.That(listOfLines[3], Is.EqualTo("<span class='scenario-keyword'>Scenario: </span>"));
            Assert.That(listOfLines[4], Is.EqualTo("<span class='scenario-name'>Scenario Name</span>"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='scenario-index'>5</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span class='scenario-duration'>1s 500ms</span>"));
            Assert.That(listOfLines[7], Is.EqualTo("</div>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName_WithoutIndexId_OmitsIndexSpan()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var scenario = new LivingDocScenario { Name = "Login" };
            var example = new LivingDocExample();

            var listOfLines = generator.GenerateDataScenarioName(scenario, example, string.Empty);

            Assert.That(listOfLines, Has.None.Contains("scenario-index"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName_WithStacktrace_RendersStacktraceButton()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var scenario = new LivingDocScenario { Name = "Failing Login" };
            var example = new LivingDocExample();
            example.Steps.Add(new LivingDocStep
            {
                Keyword = "Then",
                Name = "I see the dashboard",
                Status = LivingDocStatuses.Failed.ToString(),
                ExceptionStackTrace = "at SomeClass.SomeMethod() in File.cs:line 42"
            });

            var listOfLines = generator.GenerateDataScenarioName(scenario, example, string.Empty);

            Assert.That(listOfLines, Has.Some.Contains("scenario-stacktraces"));
            Assert.That(listOfLines, Has.Some.Contains("toggleStacktraces(this)"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName_WithHistory_RendersHistoryButton()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var scenario = new LivingDocScenario { Name = "Login" };
            var example = new LivingDocExample();
            example.History.Add(new LivingDocExampleHistoryResults
            {
                Date = DateTime.UtcNow,
                Status = LivingDocStatuses.Passed.ToString()
            });

            var listOfLines = generator.GenerateDataScenarioName(scenario, example, string.Empty);

            Assert.That(listOfLines, Has.Some.Contains("scenario-history"));
            Assert.That(listOfLines, Has.Some.Contains("toggleHistory(this)"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioName_WithAttachments_RendersAttachmentsButton()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var scenario = new LivingDocScenario { Name = "Login" };
            var example = new LivingDocExample();
            example.Attachments.Add("screenshot.png");

            var listOfLines = generator.GenerateDataScenarioName(scenario, example, string.Empty);

            Assert.That(listOfLines, Has.Some.Contains("scenario-attachments"));
            Assert.That(listOfLines, Has.Some.Contains("toggleAttachments(this)"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioSteps()
        {
            var livingDocProject = new LivingDocProject();

            var example = new LivingDocExample();

            var step = new LivingDocStep();
            step.Keyword = "Given";
            step.Name = "I have logged in to the application";

            example.Steps.Add(step);

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioSteps(example);

            Assert.That(listOfLines.Count, Is.EqualTo(10));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Steps -->"));
            Assert.That(listOfLines[5], Is.EqualTo("<span class='step-keyword'>Given</span>"));
            Assert.That(listOfLines[6], Is.EqualTo("<span class='step-name'>I have logged in to the application</span>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStep_WithDataTable_RendersDataTableBlock()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());

            var dataTable = new LivingDocDataTable();
            var row = new LivingDocDataTableRow();
            row.Cells.Add("username");
            row.Cells.Add("password");
            dataTable.Rows.Add(row);

            var step = new LivingDocStep
            {
                Keyword = "Given",
                Name = "the following users exist",
                DataTable = dataTable
            };

            var listOfLines = generator.GenerateDataScenarioStep(step);

            Assert.That(listOfLines, Does.Contain("<!-- Scenario Steps Data Table Section -->"));
            Assert.That(listOfLines, Does.Contain("<div class='steps-datatable'>"));
            Assert.That(listOfLines, Does.Contain("<table class='scenario-datatable'>"));
            Assert.That(listOfLines, Does.Contain("<td>username</td>"));
            Assert.That(listOfLines, Does.Contain("<td>password</td>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStep_Passed_RendersCheckSymbol()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                Keyword = "Then",
                Name = "I see the dashboard",
                Status = LivingDocStatuses.Passed.ToString()
            };

            var listOfLines = generator.GenerateDataScenarioStep(step);

            Assert.That(listOfLines, Has.Some.Contains("&check;"));
            Assert.That(listOfLines, Has.Some.Contains("color-passed"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStep_Failed_RendersCrossSymbol()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                Keyword = "Then",
                Name = "I see the dashboard",
                Status = LivingDocStatuses.Failed.ToString()
            };

            var listOfLines = generator.GenerateDataScenarioStep(step);

            Assert.That(listOfLines, Has.Some.Contains("&cross;"));
            Assert.That(listOfLines, Has.Some.Contains("color-failed"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepMessage_RendersMessageBox_WhenMessageExists()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep { Message = "Pending Step Definition" };

            var listOfLines = generator.GenerateDataScenarioStepMessage(step);

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Step Message -->"));
            Assert.That(listOfLines, Does.Contain("<div class='message-box'>"));
            Assert.That(listOfLines, Has.Some.Contains("Pending Step Definition"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepMessage_RendersNewlinesAsBr()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep { Message = "Line One\nLine Two" };

            var listOfLines = generator.GenerateDataScenarioStepMessage(step);

            Assert.That(listOfLines, Has.Some.Contains("Line One<br>Line Two"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepMessage_ReturnsEmpty_WhenMessageIsNull()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep { Message = null };

            Assert.That(generator.GenerateDataScenarioStepMessage(step), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepMessage_ReturnsEmpty_WhenMessageIsWhitespace()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep { Message = "   " };

            Assert.That(generator.GenerateDataScenarioStepMessage(step), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionMessage_RendersExceptionBlock_WhenBothSet()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                Status = LivingDocStatuses.Failed.ToString(),
                ExceptionType = "AssertionException",
                ExceptionMessage = "Expected true but was false"
            };

            var listOfLines = generator.GenerateDataScenarioStepExceptionMessage(step);

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Step Message -->"));
            Assert.That(listOfLines, Does.Contain("<div class='message-box'>"));
            Assert.That(listOfLines, Has.Some.Contains("message-failed"));
            Assert.That(listOfLines, Has.Some.Contains("AssertionException"));
            Assert.That(listOfLines, Has.Some.Contains("Expected true but was false"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionMessage_ReturnsEmpty_WhenExceptionTypeIsNull()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                ExceptionType = null,
                ExceptionMessage = "Some message"
            };

            Assert.That(generator.GenerateDataScenarioStepExceptionMessage(step), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionMessage_ReturnsEmpty_WhenExceptionMessageIsNull()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                ExceptionType = "AssertionException",
                ExceptionMessage = null
            };

            Assert.That(generator.GenerateDataScenarioStepExceptionMessage(step), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionStackTrace_RendersStacktraceBlock_WhenSet()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                Status = LivingDocStatuses.Failed.ToString(),
                ExceptionStackTrace = "at SomeClass.SomeMethod() in File.cs:line 42"
            };

            var listOfLines = generator.GenerateDataScenarioStepExceptionStackTrace(step);

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Step Message -->"));
            Assert.That(listOfLines, Does.Contain("<div class='message-box'>"));
            Assert.That(listOfLines, Has.Some.Contains("message-failed"));
            Assert.That(listOfLines, Has.Some.Contains("Stacktrace"));
            Assert.That(listOfLines, Has.Some.Contains("message-stacktrace"));
            Assert.That(listOfLines, Has.Some.Contains("at SomeClass.SomeMethod() in File.cs:line 42"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionStackTrace_RendersNewlinesAsBr()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep
            {
                Status = LivingDocStatuses.Failed.ToString(),
                ExceptionStackTrace = "at Method1()\nat Method2()"
            };

            var listOfLines = generator.GenerateDataScenarioStepExceptionStackTrace(step);

            Assert.That(listOfLines, Has.Some.Contains("at Method1()<br>at Method2()"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionStackTrace_ReturnsEmpty_WhenNull()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep { ExceptionStackTrace = null };

            Assert.That(generator.GenerateDataScenarioStepExceptionStackTrace(step), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioStepExceptionStackTrace_ReturnsEmpty_WhenWhitespace()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var step = new LivingDocStep { ExceptionStackTrace = "   " };

            Assert.That(generator.GenerateDataScenarioStepExceptionStackTrace(step), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioExamples()
        {
            var livingDocProject = new LivingDocProject();

            var examples = new LivingDocExample
            {
                DataTable = new LivingDocDataTable
                {
                    Rows = new List<LivingDocDataTableRow>
                    {
                        new LivingDocDataTableRow
                        {
                            Cells = new List<string> { "username", "password" }
                        },
                    }
                },
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioExamples(examples);

            Assert.That(listOfLines.Count, Is.EqualTo(17));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Examples -->"));
            Assert.That(listOfLines[8], Is.EqualTo("<td>username</td>"));
            Assert.That(listOfLines[10], Is.EqualTo("<td>password</td>"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioExamples_ReturnsEmpty_WhenNoDataTable()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var example = new LivingDocExample();

            Assert.That(generator.GenerateDataScenarioExamples(example), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioDataTable_RendersCorrectCellsAndSeparators()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());

            var dataTable = new LivingDocDataTable();
            var row = new LivingDocDataTableRow();
            row.Cells.Add("username");
            row.Cells.Add("password");
            dataTable.Rows.Add(row);

            var listOfLines = generator.GenerateDataScenarioDataTable(dataTable);

            Assert.That(listOfLines[0], Is.EqualTo("<table class='scenario-datatable'>"));
            Assert.That(listOfLines, Does.Contain("<td>username</td>"));
            Assert.That(listOfLines, Does.Contain("<td>password</td>"));
            Assert.That(listOfLines.FindAll(l => l == "<td>|</td>").Count, Is.EqualTo(3));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioAttachments()
        {
            var livingDocProject = new LivingDocProject();

            var example = new LivingDocExample
            {
                Attachments = new List<string>
                {
                    "screenshot1.png",
                    "logfile1.txt"
                }
            };

            var generator = new LivingDocDataObjectsGenerator(livingDocProject);
            var listOfLines = generator.GenerateDataScenarioAttachments(example);

            Assert.That(listOfLines.Count, Is.EqualTo(8));
            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario Attachments -->"));
            Assert.That(listOfLines[4], Does.Contain("screenshot1.png"));
            Assert.That(listOfLines[5], Does.Contain("logfile1.txt"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioAttachments_ReturnsEmpty_WhenNoAttachments()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var example = new LivingDocExample();

            Assert.That(generator.GenerateDataScenarioAttachments(example), Is.Empty);
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioAttachments_AbsoluteUrl_UsesFullPath()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var example = new LivingDocExample();
            example.Attachments.Add("https://example.com/reports/screenshot.png");

            var listOfLines = generator.GenerateDataScenarioAttachments(example);

            Assert.That(listOfLines, Has.Some.Contains("href='https://example.com/reports/screenshot.png'"));
            Assert.That(listOfLines, Has.Some.Contains(">https://example.com/reports/screenshot.png<"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioAttachments_LocalPath_UsesFilenameOnly()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var example = new LivingDocExample();
            example.Attachments.Add(@"C:\Reports\screenshots\login_failure.png");

            var listOfLines = generator.GenerateDataScenarioAttachments(example);

            Assert.That(listOfLines, Has.Some.Contains(">login_failure.png<"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioHistory_ReturnsHistory_WhenHistoryExists()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var example = new LivingDocExample();
            example.History.Add(new LivingDocExampleHistoryResults
            {
                Date = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc),
                Status = LivingDocStatuses.Passed.ToString()
            });
            example.History.Add(new LivingDocExampleHistoryResults
            {
                Date = new DateTime(2024, 6, 14, 10, 0, 0, DateTimeKind.Utc),
                Status = LivingDocStatuses.Failed.ToString()
            });

            var listOfLines = generator.GenerateDataScenarioHistory(example);

            Assert.That(listOfLines[0], Is.EqualTo("<!-- Data Scenario History -->"));
            Assert.That(listOfLines, Does.Contain("<span class='history-keyword'>History</span>"));
            Assert.That(listOfLines, Has.Some.Contains("bgcolor-passed"));
            Assert.That(listOfLines, Has.Some.Contains("bgcolor-failed"));
        }

        [Test]
        public void LivingDocDataObjectsGenerator_GenerateDataScenarioHistory_ReturnsEmpty_WhenNoHistory()
        {
            var generator = new LivingDocDataObjectsGenerator(new LivingDocProject());
            var example = new LivingDocExample();

            Assert.That(generator.GenerateDataScenarioHistory(example), Is.Empty);
        }
    }
}
