<?php
$hostname = 'localhost';
$username = 'root';
$password = '0117';
$database = 'equipments';

try
{
  $dbh = new PDO('mysql:host='.$hostname.';dbname='.$database, $username, $password);
}
catch(PDOException $e)
{
  echo '<h1>An error has occurred.</h1><pre>', $e->getMessage(),'</pre>';
}

$sth = $dbh->query('SELECT * FROM equipments ORDER BY power DESC LIMIT 5');
$sth->setFetchMode(PDO::FETCH_ASSOC);

$result = $sth->fetchAll();

if(count($result)>0)
{
  foreach($result as $r)
  {
    echo $r['name'], "\n _";
    echo $r['power'], "\n _";
    echo $r['health'], "\n _";
  }
}
?>