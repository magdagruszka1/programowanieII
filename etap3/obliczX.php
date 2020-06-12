<?php
try{
    $arrContextOptions=array(
        "ssl"=>array(
            "verify_peer"=>false,
            "verify_peer_name"=>false,
        ),
        "http" => array("ignore_errors" => true),
    );

    $formula = $_GET['formula'];
    $varX = $_GET['varX'];
    $url = 'https://localhost:5001/api/calculate?formula='.$formula.'&x='.$varX;

    $response = file_get_contents($url, false, stream_context_create($arrContextOptions));

    $json_array=json_decode($response,true);

    function display_array($json_rec) {
        if($json_rec) {
            foreach ($json_rec as $key => $value) {
                if(is_array($value)) {
                        echo '<h5>'.$key.': </h5>';
                    display_array($value);
                } else if ($value == 'ok' && !is_numeric($value)) {
                    echo '<div class="alert alert-success" role="alert">The '.$key.' is <a class="text-uppercase font-weight-bold">'.$value.'</a></div>';
                }
                else if ($value == 'error' && !is_numeric($value)) {
                    echo '<div class="alert alert-danger" role="alert">The '.$key.' is <a class="text-uppercase font-weight-bold">'.$value.'</a></div>';
                } else if (is_numeric($value)) {
                    if($key == 'result') echo '<p class="h1 text-capitalize">'.$key.'</p>';
                    echo '<samp>'.$value.'</samp> ';
                } else if (is_string($value)) {
                    echo '<samp>'.$value.'</samp> ';
                }
            }
        }
    }
    display_array($json_array);

}
catch(Exception $wyjatek)
{
    echo $wyjatek->getMessage();
}