$target = "MieGTFSConverter.exe"
$path = "\\contents-storage\horus\tools\路線バス_ダイヤ取込\三重_GTFSコンバーター\検証版\" + $target
$v1 = (Get-ItemProperty $path).VersionInfo.FileVersion
$v2 = (Get-ItemProperty $target).VersionInfo.FileVersion
if ( !( $v1 -eq $v2 ) ) {
  $result = [System.Windows.Forms.MessageBox]::Show("検証版にリリースしますか？",$v2,"YesNo","Question","Button2")
  if ($result -eq "Yes") {
    Copy-Item -Path $target -Destination $path
  }
}