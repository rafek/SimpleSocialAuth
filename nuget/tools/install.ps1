param($installPath, $toolsPath, $package, $project)

$lastBackslashInPathPosition = (Get-Project).FullName.LastIndexOf('\')
$webConfigPath = (Get-Project).FullName.Substring(0,$lastBackslashInPathPosition) + '\' + 'web.config'

$xml = [xml](Get-Content $webConfigPath)
$root = $xml.get_DocumentElement()

$root.{system.web}.authentication.forms.loginUrl = '~/SimpleAuth/LogIn'

$xml.Save($webConfigPath)

Write-Host "Forms Authentication Login Url has been changed to '~/SimpleAuth/LogIn'."