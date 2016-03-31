// ReSharper disable once UseOfImplicitGlobalInFunctionScope
var baseUrl = urlBase;
$(document).ready(function () {
    SugereHoraFinal( $("#HoraInicial").val());
    $("#HoraInicial").change(function () {
        SugereHoraFinal($(this).val());
    });
});

function SugereHoraFinal(horaIni){
    $.ajax({
        url: baseUrl + "/Horario/SugerirHoraFinal?horaInicial=" + horaIni,
        type: "GET",
    })
      .done(function (result) {
          $('#HoraFinal').empty();

          for (var i = 0; i < result.length; i++) {
              $("#HoraFinal").append($('<option/>').attr('value', result[i]).text(result[i]));
          }
      });
}