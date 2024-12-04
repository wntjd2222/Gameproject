<?php
$hostname='localhost';
$username='root';
$password='0117';
$database='equipments';
$secretKey="LJS";

try
{
  $dbh=new PDO('mysql:host='.$hostname.';dbname='.$database, $username, $password);
}
catch(PDOException $e)
{
  echo'<h1>An error has ocurred.</h1><pre>',$e->getMessage(),'</pre>';
}
$hash=$_GET['hash'];
$realHash=hash('sha256', $_GET['name'].$_GET['power'].$_GET['health'].$secretKey);

if($realHash==$hash)
{
  $sth=$dbh->prepare('INSERT INTO equipments VALUES (null, :name, :power, :health)');
  try
  {
    $sth->bindParam(':name', $_GET['name'], PDO::PARAM_STR);
    $sth->bindParam(':power', $_GET['power'], PDO::PARAM_INT);
    $sth->bindParam(':health', $_GET['health'], PDO::PARAM_INT);
    $sth->execute();
  }
  catch(Exception $e)
  {
    echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage(),'</pre>';
  }
}
?>