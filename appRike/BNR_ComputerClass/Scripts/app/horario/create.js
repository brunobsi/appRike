// ReSharper disable once UseOfImplicitGlobalInFunctionScope

$(document).ready(function () {
    var baseUrl = urlBase;
    $("#HoraInicial").change(function () {
        $.ajax({
            url: baseUrl + "/Horario/SugerirHoraFinal?horaInicial=" + $(this).val(),
            type: "GET",
        })
        .done(function (result) {
            $('#HoraFinal').empty();
            
            for (var i = 0; i < result.length; i++) {
                $("#HoraFinal").append($('<option/>').attr('value', result[i]).text(result[i]));
            }
        });
    });
});