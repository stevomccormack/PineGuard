@{
    # PineGuard tools/ lint gate (docs/ai/plans/tools-review-and-standardisation.md, T2.01).
    #
    # Only Error and Warning findings fail the eventual `Test-Tools.ps1` gate (T2.04) and CI job
    # (T2.05) — Information-level findings are reported but never block.
    Severity     = @('Error', 'Warning')

    # Called out by name because they are the rules that actively caught real bugs during
    # Phase 1's manual fixes: unapproved verbs (§3.2 renames), automatic-variable shadowing,
    # missing comment-based help (§2.x), and the indentation/whitespace conventions below.
    IncludeRules = @(
        'PSUseApprovedVerbs',
        'PSAvoidAssignmentToAutomaticVariable',
        'PSProvideCommentHelp',
        'PSUseConsistentIndentation',
        'PSUseConsistentWhitespace',
        'PSAvoidTrailingWhitespace'
    )

    # PSAvoidUsingWriteHost: deliberate, permanent exclusion. docs/ai/specs/tools/spec.md §2.3
    # mandates colored console output via Write-Host for status messages across all tools/
    # scripts (Cyan for activity start, Green for success, Yellow for warnings/skips). Every
    # Phase 1 verification report already noted these findings as expected/pre-existing; this
    # entry makes the exclusion explicit instead of leaving the rule simply un-enabled.
    ExcludeRules = @(
        'PSAvoidUsingWriteHost'
    )

    Rules        = @{
        # §3.4 Structural rules: UTF-8 without BOM repo-wide for .ps1/.psm1/.psd1. This rule
        # wants a BOM, which contradicts that decision, so it stays off rather than on.
        PSUseBOMForUnicodeEncodedFile = @{
            Enable = $false
        }

        # Matches .editorconfig's [*.{ps1,psm1,psd1}] convention: indent_style = space,
        # indent_size = 4 (root defaults, ../.editorconfig lines 10-12, 34-35).
        PSUseConsistentIndentation    = @{
            Enable              = $true
            IndentationSize     = 4
            Kind                = 'space'
            PipelineIndentation = 'IncreaseIndentationForFirstPipeline'
        }

        PSUseConsistentWhitespace     = @{
            Enable                          = $true
            CheckInnerBrace                 = $true
            CheckOpenBrace                  = $true
            CheckOpenParen                  = $true
            CheckOperator                   = $true
            CheckPipe                       = $true
            CheckPipeForRedundantWhitespace = $false
            CheckSeparator                  = $true
            CheckParameter                  = $false
        }
    }
}
