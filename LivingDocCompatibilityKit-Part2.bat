echo off

cd .\Expressium.LivingDoc.UnitTests\bin\Debug\net8.0

del .\CCK\Samples\All-Statuses\AllStatuses.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\all-statuses\all-statuses.ndjson --output .\CCK\Samples\All-Statuses\AllStatuses.html --title "Compatibility Kit All Statuses"
start .\CCK\Samples\All-Statuses\AllStatuses.html

del .\CCK\Samples\Cdata\Cdata.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\cdata\cdata.ndjson --output .\CCK\Samples\Cdata\Cdata.html --title "Compatibility Kit CData"
start .\CCK\Samples\Cdata\Cdata.html

del .\CCK\Samples\Doc-Strings\DocStrings.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\doc-strings\doc-strings.ndjson --output .\CCK\Samples\Doc-Strings\DocStrings.html --title "Compatibility Kit Doc Strings"
start .\CCK\Samples\Doc-Strings\DocStrings.html

del .\CCK\Samples\Examples-Tables-Attachment\ExamplesTablesAttachment.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\examples-tables-attachment\examples-tables-attachment.ndjson --output .\CCK\Samples\Examples-Tables-Attachment\ExamplesTablesAttachment.html --title "Compatibility Kit Examples Tables Attachment"
start .\CCK\Samples\Examples-Tables-Attachment\ExamplesTablesAttachment.html

del .\CCK\Samples\Examples-Tables-Undefined\ExamplesTablesUndefined.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\examples-tables-undefined\examples-tables-undefined.ndjson --output .\CCK\Samples\Examples-Tables-Undefined\ExamplesTablesUndefined.html --title "Compatibility Kit Examples Tables Undefined"
start .\CCK\Samples\Examples-Tables-Undefined\ExamplesTablesUndefined.html

del .\CCK\Samples\Failedish-Combinations\FailedishCombinations.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\failedish-combinations\failedish-combinations.ndjson --output .\CCK\Samples\Failedish-Combinations\FailedishCombinations.html --title "Compatibility Kit Failedish Combinations"
start .\CCK\Samples\Failedish-Combinations\FailedishCombinations.html

del .\CCK\Samples\Global-Hooks\GlobalHooks.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\global-hooks\global-hooks.ndjson --output .\CCK\Samples\Global-Hooks\GlobalHooks.html --title "Compatibility Kit Global Hooks"
start .\CCK\Samples\Global-Hooks\GlobalHooks.html

del .\CCK\Samples\Global-Hooks-Afterall-Error\GlobalHooksAfterallError.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\global-hooks-afterall-error\global-hooks-afterall-error.ndjson --output .\CCK\Samples\Global-Hooks-Afterall-Error\GlobalHooksAfterallError.html --title "Compatibility Kit Global Hooks Afterall Error"
start .\CCK\Samples\Global-Hooks-Afterall-Error\GlobalHooksAfterallError.html

del .\CCK\Samples\Global-Hooks-Attachments\GlobalHooksAttachments.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\global-hooks-attachments\global-hooks-attachments.ndjson --output .\CCK\Samples\Global-Hooks-Attachments\GlobalHooksAttachments.html --title "Compatibility Kit Global Hooks Attachments"
start .\CCK\Samples\Global-Hooks-Attachments\GlobalHooksAttachments.html

del .\CCK\Samples\Global-Hooks-Beforeall-Error\GlobalHooksBeforeallError.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\global-hooks-beforeall-error\global-hooks-beforeall-error.ndjson --output .\CCK\Samples\Global-Hooks-Beforeall-Error\GlobalHooksBeforeallError.html --title "Compatibility Kit Global Hooks Beforeall Error"
start .\CCK\Samples\Global-Hooks-Beforeall-Error\GlobalHooksBeforeallError.html

del .\CCK\Samples\Hooks-Attachment\HooksAttachment.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\hooks-attachment\hooks-attachment.ndjson --output .\CCK\Samples\Hooks-Attachment\HooksAttachment.html --title "Compatibility Kit Hooks Attachment"
start .\CCK\Samples\Hooks-Attachment\HooksAttachment.html

del .\CCK\Samples\Hooks-Conditional\HooksConditional.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\hooks-conditional\hooks-conditional.ndjson --output .\CCK\Samples\Hooks-Conditional\HooksConditional.html --title "Compatibility Kit Hooks Conditional"
start .\CCK\Samples\Hooks-Conditional\HooksConditional.html

del .\CCK\Samples\Hooks-Named\HooksNamed.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\hooks-named\hooks-named.ndjson --output .\CCK\Samples\Hooks-Named\HooksNamed.html --title "Compatibility Kit Hooks Named"
start .\CCK\Samples\Hooks-Named\HooksNamed.html

del .\CCK\Samples\Hooks-Skipped\HooksSkipped.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\hooks-skipped\hooks-skipped.ndjson --output .\CCK\Samples\Hooks-Skipped\HooksSkipped.html --title "Compatibility Kit Hooks Skipped"
start .\CCK\Samples\Hooks-Skipped\HooksSkipped.html

del .\CCK\Samples\Hooks-Undefined\HooksUndefined.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\hooks-undefined\hooks-undefined.ndjson --output .\CCK\Samples\Hooks-Undefined\HooksUndefined.html --title "Compatibility Kit Hooks Undefined"
start .\CCK\Samples\Hooks-Undefined\HooksUndefined.html

del .\CCK\Samples\Markdown\Markdown.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\markdown\markdown.ndjson --output .\CCK\Samples\Markdown\Markdown.html --title "Compatibility Kit Markdown"
start .\CCK\Samples\Markdown\Markdown.html

del .\CCK\Samples\Multiple-Features\MultipleFeatures.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\multiple-features\multiple-features.ndjson --output .\CCK\Samples\Multiple-Features\MultipleFeatures.html --title "Compatibility Kit Multiple Features"
start .\CCK\Samples\Multiple-Features\MultipleFeatures.html

del .\CCK\Samples\Multiple-Features-Reversed\MultipleFeaturesReversed.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\multiple-features-reversed\multiple-features-reversed.ndjson --output .\CCK\Samples\Multiple-Features-Reversed\MultipleFeaturesReversed.html --title "Compatibility Kit Multiple Features Reversed"
start .\CCK\Samples\Multiple-Features-Reversed\MultipleFeaturesReversed.html

del .\CCK\Samples\Parameter-Types\ParameterTypes.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\parameter-types\parameter-types.ndjson --output .\CCK\Samples\Parameter-Types\ParameterTypes.html --title "Compatibility Kit Parameter Types"
start .\CCK\Samples\Parameter-Types\ParameterTypes.html

del .\CCK\Samples\Pending-Exception\PendingException.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\pending-exception\pending-exception.ndjson --output .\CCK\Samples\Pending-Exception\PendingException.html --title "Compatibility Kit Pending Exception"
start .\CCK\Samples\Pending-Exception\PendingException.html

del .\CCK\Samples\Regular-Expression\RegularExpression.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\regular-expression\regular-expression.ndjson --output .\CCK\Samples\Regular-Expression\RegularExpression.html --title "Compatibility Kit Regular Expression"
start .\CCK\Samples\Regular-Expression\RegularExpression.html

del .\CCK\Samples\Retry-Ambiguous\RetryAmbiguous.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\retry-ambiguous\retry-ambiguous.ndjson --output .\CCK\Samples\Retry-Ambiguous\RetryAmbiguous.html --title "Compatibility Kit Retry Ambiguous"
start .\CCK\Samples\Retry-Ambiguous\RetryAmbiguous.html

del .\CCK\Samples\Retry-Pending\RetryPending.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\retry-pending\retry-pending.ndjson --output .\CCK\Samples\Retry-Pending\RetryPending.html --title "Compatibility Kit Retry Pending"
start .\CCK\Samples\Retry-Pending\RetryPending.html

del .\CCK\Samples\Retry-Undefined\RetryUndefined.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\retry-undefined\retry-undefined.ndjson --output .\CCK\Samples\Retry-Undefined\RetryUndefined.html --title "Compatibility Kit Retry Undefined"
start .\CCK\Samples\Retry-Undefined\RetryUndefined.html

del .\CCK\Samples\Skipped-Exception\SkippedException.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\skipped-exception\skipped-exception.ndjson --output .\CCK\Samples\Skipped-Exception\SkippedException.html --title "Compatibility Kit Skipped Exception"
start .\CCK\Samples\Skipped-Exception\SkippedException.html

del .\CCK\Samples\Skipped-Failing-Hook\SkippedFailingHook.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\skipped-failing-hook\skipped-failing-hook.ndjson --output .\CCK\Samples\Skipped-Failing-Hook\SkippedFailingHook.html --title "Compatibility Kit Skipped Failing Hook"
start .\CCK\Samples\Skipped-Failing-Hook\SkippedFailingHook.html

del .\CCK\Samples\Test-Run-Exception\TestRunException.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\test-run-exception\test-run-exception.ndjson --output .\CCK\Samples\Test-Run-Exception\TestRunException.html --title "Compatibility Kit Test Run Exception"
start .\CCK\Samples\Test-Run-Exception\TestRunException.html

del .\CCK\Samples\Undefined\Undefined.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\undefined\undefined.ndjson --output .\CCK\Samples\Undefined\Undefined.html --title "Compatibility Kit Undefined"
start .\CCK\Samples\Undefined\Undefined.html

del .\CCK\Samples\Unknown-Parameter-Type\UnknownParameterType.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\unknown-parameter-type\unknown-parameter-type.ndjson --output .\CCK\Samples\Unknown-Parameter-Type\UnknownParameterType.html --title "Compatibility Kit Unknown Parameter Type"
start .\CCK\Samples\Unknown-Parameter-Type\UnknownParameterType.html

del .\CCK\Samples\Unused-Steps\UnusedSteps.html
Expressium.LivingDoc.Cli.exe --generate --input .\CCK\Samples\unused-steps\unused-steps.ndjson --output .\CCK\Samples\Unused-Steps\UnusedSteps.html --title "Compatibility Kit Unused Steps"
start .\CCK\Samples\Unused-Steps\UnusedSteps.html
