// ReSharper disable once UseOfImplicitGlobalInFunctionScope

$(document).ready(function () {
    var baseUrl = urlBase;

    $("input[name='TipoRadio']").change(function () {
        var filtro = $("input[name='TipoRadio']:checked").val();

        $.ajax({
            url: baseUrl + "/Horario/AtualizaSelectFiltro?filtro=" + filtro,
            type: "GET",
        })
        .done(function (result) {
            $("#divAgendas").html(result);
            registerChangeSelect();
        });
    });

    registerChangeSelect();

    function registerChangeSelect() {
        $("#Select").change(function () {
            var filtro = $("input[name='TipoRadio']:checked").val();
            $.ajax({
                url: baseUrl + "/Horario/AtualizaHorariosIndex?filtro=" + filtro +"&value="+ $(this).val(),
                type: "GET",
            })
            .done(function (result) {
                $("#divAgendas").html(result);
                registerChangeSelect();
            });
        });
    }
});




