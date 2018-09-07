
Param(
	[string]$projectName,
	[string]$filePath
)

# initialize the items variable with the
# contents of a directory
$items = Get-ChildItem

$skipList = "bin", "obj", ".git",".vs","create.ps1","README.md", "PodizanjeProjekta.txt", "packages"

if(!(Test-Path $filePath)){
    New-Item -ItemType Directory -Force -path $filePath

}


# enumerate the items array
foreach ($item in $items)
{
	if($skipList -contains $item.Name){
		continue;
	}				
	Copy-Item $item $filePath -recurse -force
}

$folders = Get-ChildItem -Path $filePath -recurse -Directory | Sort-Object -Descending FullName


foreach($copiedItem in $folders)
{
	if($copiedItem.Attributes -match ‘Directory’){
		$newName = $copiedItem.Name -replace 'UmbracoStarter', $projectName
	    if($newName -ne $copiedItem.Name){
            Rename-Item -Path $copiedItem.FullName -NewName $newName
        }
	}
}

 $files = Get-ChildItem -Path $filePath -recurse -File | Sort-Object -Descending FullName

 foreach($file in $files)
 {
    if($file.Name -ne 'packages.config'){
         (Get-Content $file.FullName) -replace 'UmbracoStarter', $projectName | out-file -encoding utf8 $file.FullName
         $newName = $file.Name -replace 'UmbracoStarter', $projectName
         if($newName -ne $file.Name){
             Rename-Item -Path $file.FullName -NewName $newName
         }
     }

 }
