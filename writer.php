<?php
$filename='groundtrack.latlon';

$t=$_REQUEST["t"];
$string=$_REQUEST["s"];

if ($t === "create")
{
  if (is_file($filename))
  {
    unlink($filename);
  }
  echo "Successful create!"
  exit;
}
if ($t === "append")
{
  file_put_contents($filename, $string ,FILE_APPEND);
  echo "Successful append!"
  exit;
}

if ($t === "save")
{
  header('Content-Type: application/octet-stream');
  header('Content-Disposition: attachment; filename='.basename($filename));
  header('Expires: 0');
  header('Cache-Control: must-revalidate');
  header('Pragma: public');
  header('Content-Length: ' . filesize($filename));
  readfile($filename);
  echo "Successful save!"
  exit;
}
?>
