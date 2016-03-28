// ReSharper disable once UseOfImplicitGlobalInFunctionScope

$(document).ready(function () {
    var baseUrl = urlBase;

    registerChangeSelect();

    function registerChangeSelect() {
        $("#Select").change(function () {
            $.ajax({
                url: baseUrl + "/Horario/AtualizaHorariosIndex?value="+ $(this).val(),
                type: "GET",
            })
            .done(function (result) {
                $("#divAgendas").html(result);
                registerChangeSelect();
            });
        });
    }
});




