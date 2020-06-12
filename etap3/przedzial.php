<!DOCTYPE html>
<html lang="pl">
  <head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>

    <title>WolframBeta</title>

    <script type="text/javascript">
      window.addEventListener('keydown',function(e){if(e.keyIdentifier=='U+000A'||e.keyIdentifier=='Enter'||e.keyCode==13){if(e.target.nodeName=='INPUT'&&e.target.type=='text'){e.preventDefault();return false;}}},true);
    </script>

    <script>
      function wyslij_token() {
        var formula=$('#formula').val();
        var poczatekprzedzialu=$('#poczatekprzedzialu').val();
        var koniecprzedzialu=$('#koniecprzedzialu').val();
        var nrazy=$('#nrazy').val();
        $.get("obliczPrzedzial.php",{formula:formula,poczatekprzedzialu:poczatekprzedzialu,koniecprzedzialu:koniecprzedzialu,nrazy:nrazy},
        function(data) {
          $("#json_response").html(data);
        });
      }
    </script>
  </head>
  <body>
    <h1>WolframBeta</h1>

    <ul>
      <li>
        <a href="index.php">Token</a>
      </li>
      <li>
        <a href="obliczanie.php">Obliczanie</a>
      </li>
      <li>
        <a href="przedzial.php">Przedział</a>
      </li>
    </ul>
         
    <form>
      <span>Wpisz formułe</span>
        <input type="text" id="formula"><br><br>
      <span>Początek przedziału</span>
        <input type="text" id="poczatekprzedzialu" name="poczatekprzedzialu"><br><br>
      <span>Koniec przedziału</span>
        <input type="text" id="koniecprzedzialu" name="koniecprzedzialu"><br><br>
      <span>N-razy</span>
        <input type="text" id="nrazy" name="nrazy"><br><br>
        <input id="button" type="button" value="Wyślij" onclick="wyslij_token()" />
    </form>
    
    <div id="json_response">
   </div>

    <script src="https://code.jquery.com/jquery-3.1.1.min.js"></script>

  </body>
</html>

