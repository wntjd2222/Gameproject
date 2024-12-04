<?php
$hostname = 'localhost';
$username = 'root';
$password = '0117';
$database = 'logindata';

try
{
  $dbh = new PDO('mysql:host='.$hostname.';dbname='.$database, $username, $password);
}
catch(PDOException $e)
{
  echo '<h1>An error has occurred.</h1><pre>', $e->getMessage(),'</pre>';
}

$check = $dbh->query("SELECT * FROM logindatas WHERE ".$ID." MEMBER OF(custinfo->'$.zipcod'");

$numrows = mysql_num_rows($check);
if($numrows == 0)
{
	die("ID does not exist. \n");
}
else
{
  while($row = mysql_fetch_assoc($check))
  {
    if($pass == $row['PW'])
    {
      echo(" " .$row ['info'], " \n");
      die("Login-Success");
    }
    else
      die("Pass does not Match. \n");
  }
}
?>