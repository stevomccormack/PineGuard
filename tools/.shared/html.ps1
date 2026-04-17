<#
.SYNOPSIS
    Shared HTML report helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Write-RedirectHtml and Write-OptionalRedirectHtml
    into the calling script's scope.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-RedirectHtml {
    <#
    .SYNOPSIS
        Generates an HTML redirect page with auto-redirect on successful fetch.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $OutputPath,

        [Parameter(Mandatory)]
        [string] $Title,

        [Parameter(Mandatory)]
        [string] $TargetRelativePath,

        [string] $NotFoundMessage = 'Report has not been generated yet.'
    )

    $escapedTitle = [System.Net.WebUtility]::HtmlEncode($Title)
    $escapedTarget = [System.Net.WebUtility]::HtmlEncode($TargetRelativePath)
    $escapedNotFoundMessage = [System.Net.WebUtility]::HtmlEncode($NotFoundMessage)

    $html = @"
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>$escapedTitle</title>
  <meta http-equiv="refresh" content="0; url=$escapedTarget" />
  <style>
    body { font-family: system-ui, -apple-system, Segoe UI, Roboto, Arial, sans-serif; padding: 24px; }
    code { background: #f4f4f4; padding: 2px 6px; border-radius: 4px; }
  </style>
</head>
<body>
  <h1>$escapedTitle</h1>
  <p>Redirecting to: <a href="$escapedTarget"><code>$escapedTarget</code></a></p>
  <script>
    (function () {
      const url = "$escapedTarget";
      fetch(url, { method: 'HEAD' }).then(r => {
        if (!r.ok) throw new Error('missing');
        window.location.replace(url);
      }).catch(() => {
                document.body.insertAdjacentHTML('beforeend', '<p><strong>$escapedNotFoundMessage</strong></p>');
      });
    })();
  </script>
</body>
</html>
"@

    Set-Content -LiteralPath $OutputPath -Value $html -Encoding UTF8
}

function Write-OptionalRedirectHtml {
    <#
    .SYNOPSIS
        Generates an HTML redirect page without auto-redirect (file:// safe).
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $OutputPath,

        [Parameter(Mandatory)]
        [string] $Title,

        [Parameter(Mandatory)]
        [string] $TargetRelativePath,

        [string] $NotFoundMessage = 'Report has not been generated yet.'
    )

    $escapedTitle = [System.Net.WebUtility]::HtmlEncode($Title)
    $escapedTarget = [System.Net.WebUtility]::HtmlEncode($TargetRelativePath)
    $escapedNotFoundMessage = [System.Net.WebUtility]::HtmlEncode($NotFoundMessage)

    $html = @"
<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>$escapedTitle</title>
    <style>
        body { font-family: system-ui, -apple-system, Segoe UI, Roboto, Arial, sans-serif; padding: 24px; }
        code { background: #f4f4f4; padding: 2px 6px; border-radius: 4px; }
    </style>
</head>
<body>
    <h1>$escapedTitle</h1>
    <p>Open: <a href="$escapedTarget"><code>$escapedTarget</code></a></p>
    <p><strong>$escapedNotFoundMessage</strong></p>
</body>
</html>
"@

    Set-Content -LiteralPath $OutputPath -Value $html -Encoding UTF8
}
