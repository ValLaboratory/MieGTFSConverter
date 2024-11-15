$target = "MieGTFSConverter.exe"
$path = "\\contents-storage\horus\tools\�H���o�X_�_�C���捞\�O�d_GTFS�R���o�[�^�[\���ؔ�\" + $target
$v1 = (Get-ItemProperty $path).VersionInfo.FileVersion
$v2 = (Get-ItemProperty $target).VersionInfo.FileVersion
if ( !( $v1 -eq $v2 ) ) {
  $result = [System.Windows.Forms.MessageBox]::Show("���ؔłɃ����[�X���܂����H",$v2,"YesNo","Question","Button2")
  if ($result -eq "Yes") {
    Copy-Item -Path $target -Destination $path
  }
}