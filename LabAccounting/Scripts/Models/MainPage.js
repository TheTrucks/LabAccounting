var Fetching = new Boolean(false);
var SampleAddPageLoader = null;
var TemplateLoader = null;
var NameMenu = null;
var ContractMenu = null;
var Templates = [];
var ContractTemplates = [];
var ClassList = {};
var UnitList = {};
var PickersList = [];
var Validator = null;
var Paginator = {
    up: 1,
    down: 1
};
var Pagers = {};

$(document).ready(function () {
    Pagers = {
        up: $("#DataLoadTop"),
        down: $("#DataLoadBot")
    }
    Paginator.up = $("#Page").val() - 1;
    Paginator.down = Paginator.up;
    GetNewPage("down", true);
    Pagers.up.click(function () { GetNewPage("up", false); });
    Pagers.down.click(function () { GetNewPage("down", false); });

    $("#SampleAdd").on("show.bs.modal", function (modal_elem) {
        var MBody = $("#SampleAddBody");
        $(MBody).html(
            $("<div>")
                .addClass("center-block")
                .addClass("dummy-pageloader")
                .append(
                    $("<div>")
                        .addClass("pageloader")
                        .css("font-size", "10px"))
        );
        GetSampleAddPage(MBody);
    });
    $("#SampleAdd").on("hide.bs.modal", function (modal_elem) {
        ClearModal(modal_elem.target, true);
        if (SampleAddPageLoader != null)
            SampleAddPageLoader.abort("Cancelling modal");
    });
});

function GetSampleAddPage(element) {
    SampleAddPageLoader = $.ajax({
        url: MetaDataUrl,
        method: "POST"
    })
        .done(function (data) {
            $(element).html(data);
            GetTemplates();
            Validator = new FormValidator(element);
            ApplyModalBindings();
            CreatePickers();
            UpdateMetaData();
        })
        .fail(function (e) { console.log(e); }) // todo change temprorary output
        .always(function () { SampleAddPageLoader = null; });
}

function CreatePickers() {
    var ElemBody = $("#SampleAddBody");
    var AllPickers = ElemBody.find(".datepicker");
    AllPickers.each(function (ind) {
        PickersList.push(new Litepicker({
            element: AllPickers[ind],
            lang: "ru-RU",
            format: "DD.MM.YYYY",
            autoRefresh: true
        }));
    });
}

function UpdateMetaData() {
    UnitList = JSON.parse($("#Units").val());
    ClassList = JSON.parse($("#Classes").val());
}

function ApplyModalBindings() {
    var NameInput = $("#SampleAddName");
    var ContractInput = $("#SampleAddContract");
    var ContractDateInput = $("#SampleAddContractDate");

    var AllInputsList = [NameInput, ContractInput, ContractDateInput];

    NameMenu = new AutofillMenu(
        NameMenuClick,
        function (inp) { return (obj) => obj.name.startsWith(inp) },
        null
    );
    NameInput.on("keyup", function (event) { NameMenu.Draw(event); });
    NameInput.on("click", function (event) { NameMenu.Draw(event); });

    ContractMenu = new AutofillMenu(
        ContractMenuClick,
        function (inp) {
            return (obj) =>
                inp[0].length > 2 &&
                inp[1].length > 0 &&
                obj.contract.startsWith(inp[0]) &&
                obj.contractdate == inp[1]
        },
        [ContractInput, ContractDateInput]
    );
    ContractInput.on("keyup", function (event) { ContractMenu.Draw(event); });
    ContractInput.on("click", function (event) { ContractMenu.Draw(event); });
    ContractDateInput.on("keyup", function (event) { ContractMenu.Draw(event); });
    ContractDateInput.on("change", function (event) { ContractMenu.Draw(event); });
    ContractDateInput.on("click", function (event) { ContractMenu.Draw(event); });

    $('body').on('click', function (event) {
        if (!AllInputsList.find(obj => event.target == obj[0])) {
            NameMenu.RemoveMenu();
            ContractMenu.RemoveMenu();
        }
    });

    // todo binding on select class change (for "custom" variant)
    $("#SampleAddSave").on("click", function () { SaveNewSample(); })
    // todo binding on cancel button (cancelling confirmation)
}

function ClearModal(modal_elem, triggered) {
    if (!triggered) {
        $(modal_elem).modal("hide");
    }
    $(modal_elem).find("div.modal-body").children().remove();
    PickersList.forEach(picker => picker.destroy());
    PickersList = [];

    Validator = null;

    $('body').off("click");
    if (NameMenu != null) {
        NameMenu.RemoveMenu();
        NameMenu = null;
    }
    if (ContractMenu != null) {
        ContractMenu.RemoveMenu();
        ContractMenu = null;
    }
}

function NameMenuClick(element) {
    var Attr = $(element).attr("data-type");
    if (Attr && Attr != "-1") {
        var TmpTemplate = Template.FromElement(element);
        $("#SampleAddClass").val(TmpTemplate.type);
        $("#SampleAddName").val(TmpTemplate.name);
        if (TmpTemplate.precursor == true) { $("#SampleAddPrecursor").prop("checked", "true"); }
        $("#SampleAddDefUnit").val(TmpTemplate.defunit);
        $("#SampleAddStdInfo").val(TmpTemplate.stdinfo);
        $("#SampleAddStdNum").val(TmpTemplate.stdnumber);
    }
}

function ContractMenuClick(element) {
    var Attr = $(element).attr("data-contract");
    if (Attr && Attr != "") {
        var TmpTemplate = ContractTemplate.FromElement(element);
        $("#SampleAddContract").val(TmpTemplate.contract);
        $("#SampleAddContractDate").val(TmpTemplate.contractdate);
        $("#SampleAddWaybill").val(TmpTemplate.waybill);
        $("#SampleAddWaybillDate").val(TmpTemplate.waybilldate);
        $("#SampleAddSupplier").val(TmpTemplate.supplier);
        $("#SampleAddDateReceived").val(TmpTemplate.supplydate);
    }
}

function GetTemplates() {
    TemplateLoader = $.ajax({
        url: TemplatesUrl,
        type: "POST",
        dataType: "json"
    })
        .done(function (data) {
            if (data.length > 0) {
                var LoadedTemplates = JSON.parse(data);
                Templates = (LoadedTemplates.NameTemplates && LoadedTemplates.NameTemplates.length > 0) ? Template.FromJsonArray(LoadedTemplates.NameTemplates) : [];
                ContractTemplates = (LoadedTemplates.ContractTemplates && LoadedTemplates.ContractTemplates.length > 0) ? ContractTemplate.FromJsonArray(LoadedTemplates.ContractTemplates) : [];
                if (NameMenu != null) { NameMenu.SetTemplates(Templates); }
                if (ContractMenu != null) { ContractMenu.SetTemplates(ContractTemplates); }
            }
        })
        .fail(function (data) { console.log(data); }) // todo change temprorary output
        .always(function () { TemplateLoader = null; });
}

function GetNewPage(direction, initial, reload) {
    if (Fetching == false) {
        Fetching = true;
        var Scroller = direction == "down" ? Pagers.down : Pagers.up;
        LineLoad(Scroller);

        if (reload == true) {
            Paginator.down = 1;
            Paginator.up = 1;
        }
        var Page = direction == "down" ? Paginator.down : Paginator.up;

        $.ajax({
            url: SampleListUrl,
            data: JSON.stringify({ page: Page, dir: direction, order: "" }),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
            .done(function (data) {
                if (data) {
                    var JsonData = data;
                    if (JsonData.Page) {
                        if (direction == "down")
                            Paginator.down = JsonData.Page;
                        else
                            Paginator.up = JsonData.Page;
                    }
                    if (JsonData.Samples)
                        AddEntries(JsonData.Samples, Scroller, direction, reload);
                }
            })
            .fail(function (data) { console.log(data); }) // todo change temprorary output
            .always(function () {
                Fetching = false;
                if (initial == true) { $("#load").remove(); $("body").css("pointer-events", ""); }
                LineLoadEnd(Scroller);
            });
    }
}
//$(datad).html((window.innerHeight + window.pageYOffset) + "\\" + document.body.scrollHeight);
function AddEntries(samples, element, direction, reload) {
    if (samples.length > 0) {
        if (reload == true) {
            $(Pagers.up).addClass("hidden");

            $("#SampleTable > tbody > tr.dataline").remove();
        }
        else if (direction == "down") {
            Paginator.down += 1;
        }
        else {
            Paginator.up -= 1;
            if (Paginator.up < 1) {
                $(element).addClass("hidden");
            }
        }

        direction == "down" ? $("#SampleTable > tbody tr:last-child").before(samples) : $("#SampleTable > tbody tr:first-child").after(samples);
    }
}

function SaveNewSample() {
    if (Validator && Validator.validate() == true) {
        var TmpSample = SampleAddModel.FromInputs($("#SampleAddBody")); //todo check if succesful

        $.ajax({
            url: SampleSaveUrl,
            data: JSON.stringify(TmpSample),
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
            .done(function (data) {
                if (data && data.code == 200) {
                    ClearModal($("#SampleAdd"));
                    GetNewPage("down", false, true);
                }
            })
            .fail(function (data) { console.log(data); }) //todo better logging
    }
}

function LineLoad(element) {
    var Insert = $("<div>").addClass("lds-ellipsis").append($("<div>")).append($("<div>")).append($("<div>")).append($("<div>"));
    $(element).children().html("");
    $(element).children().append(Insert);
}

function LineLoadEnd(element) {
    $(element).children().html("Загрузить данные");
}

function SampleAddModel() {
    this.Category = -1;
    this.AggrState = -1;
    this.Name = "";
    this.Precursor = false;
    this.Class = -1;
    this.DefUnit = -1;
    this.StdNum = "";
    this.StdInfo = "";
    this.Quantity = -1;
    this.Comment = "";
    this.Supplier = "";
    this.Waybill = "";
    this.Contract = "";
    this.WaybillDate = "";
    this.ContractDate = "";
    this.Batch = "";
    this.ReceivedDate = "";
    this.CreatedDate = "";
    this.ExpirationDate = "";
}
SampleAddModel.FromInputs = function (modal_body) {
    var Result = new SampleAddModel();
    Result.Category = $(modal_body).find("input[name='optCategories']:checked").val();
    Result.AggrState = $(modal_body).find("input[name='optAggrs']:checked").val();
    Result.Name = $(modal_body).find("#SampleAddName").first().val();
    Result.Precursor = $(modal_body).find("#SampleAddPrecursor").first().prop("checked");
    Result.Class = $(modal_body).find("#SampleAddClass").first().val();
    Result.DefUnit = $(modal_body).find("#SampleAddDefUnit").first().val();
    Result.StdNum = $(modal_body).find("#SampleAddStdNum").first().val();
    Result.StdInfo = $(modal_body).find("#SampleAddStdInfo").first().val();
    Result.Quantity = $(modal_body).find("#SampleAddQuantity").first().val();
    Result.Comment = $(modal_body).find("#SampleAddComment").first().val();
    Result.Supplier = $(modal_body).find("#SampleAddSupplier").first().val();
    Result.Waybill = $(modal_body).find("#SampleAddWaybill").first().val();
    Result.Contract = $(modal_body).find("#SampleAddContract").first().val();
    Result.WaybillDate = $(modal_body).find("#SampleAddWaybillDate").first().val();
    Result.ContractDate = $(modal_body).find("#SampleAddContractDate").first().val();
    Result.Batch = $(modal_body).find("#SampleAddBatchNumber").first().val();
    Result.ReceivedDate = $(modal_body).find("#SampleAddDateReceived").first().val();
    Result.CreatedDate = $(modal_body).find("#SampleAddDateCreated").first().val();
    Result.ExpirationDate = $(modal_body).find("#SampleAddDateExpiration").first().val();
    return Result;
}
    // todo break script section into files