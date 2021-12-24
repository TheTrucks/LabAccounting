function Template() {
    this.type = -1;
    this.name = "";
    this.precursor = false;
    this.defunit = -1;
    this.stdinfo = "";
    this.stdnumber = "";

    Template.prototype.GetJson = function () {
        return JSON.stringify({
            "Type": this.type,
            "Name": this.name,
            "Precursor": this.precursor,
            "DefUnit": this.defunit,
            "StdInfo": this.stdinfo,
            "StdNumber": this.stdnumber
        })
    }

    Template.prototype.GetAutoElem = function () {
        return $("<div>")
            .addClass("auto-elem")
            .attr("data-type", this.type)
            .attr("data-name", this.name)
            .attr("data-prec", this.precursor)
            .attr("data-unit", this.defunit)
            .attr("data-info", this.stdinfo)
            .attr("data-num", this.stdnumber)
            .append($("<div>").append(
                $("<p>")
                    .append($("<b>").html("Наим.: "))
                    .append(this.name.length > 12 ? this.name.slice(0, 10) + "..." : this.name)
                    .append(" (" + ClassList.filter(x => x.Id == this.type)[0].ShortName + "); ")
                    .append($("<b>").html("Прекурсор: "))
                    .append(this.precursor == true ? "Да; " : "Нет; ")
                    .append($("<b>").html("Ед.изм.: "))
                    .append(UnitList.filter(x => x.Id == this.defunit)[0].ShortName)
            ).append(
                $("<p>")
                    .append($("<b>").html("№ стд.: "))
                    .append((this.stdnumber.length > 10 ? this.stdnumber.slice(0, 8) + "..." : this.stdnumber) + "; ")
                    .append($("<b>").html("Рекв.метод.исп.: "))
                    .append(this.stdinfo.length > 18 ? this.stdinfo.slice(0, 16) + "..." : this.stdinfo)
            ));
    }
}
Template.FromElement = function (element) {
    var Result = new Template();
    Result.type = $(element).attr("data-type");
    Result.name = $(element).attr("data-name");
    Result.precursor = $(element).attr("data-prec");
    Result.defunit = $(element).attr("data-unit");
    Result.stdinfo = $(element).attr("data-info");
    Result.stdnumber = $(element).attr("data-num");
    return Result;
}
Template.FromInputs = function (form, type, name, precursor, defunit, stdinfo, stdnumber) {
    var Result = new Template();
    Result.type = $(form).find($("#" + type)).val();
    Result.name = $(form).find($("#" + name)).val();
    Result.precursor = $(form).find($("#" + precursor)).prop("checked");
    Result.defunit = $(form).find($("#" + defunit)).val();
    Result.stdinfo = $(form).find($("#" + stdinfo)).val();
    Result.stdnumber = $(form).find($("#" + stdnumber)).val();

    return Result;
}
Template.FromJsonArray = function (jsonarr) {
    var Result = [];
    for (var ind = 0; ind < jsonarr.length; ind++) {
        Result.push(Template.FromJson(jsonarr[ind]));
    }
    return Result;
}
Template.FromJson = function (json) {
    var Result = new Template();
    Result.type = json.Type.Id;
    Result.name = json.Name;
    Result.precursor = json.Precursor;
    Result.defunit = json.DefaultUnit.Id;
    Result.stdinfo = json.StandartInfo;
    Result.stdnumber = json.StandartNumber;
    return Result;
}

function ContractTemplate() {
    this.contract = "";
    this.contractdate = "";
    this.waybill = "";
    this.waybilldate = "";
    this.supplier = "";
    this.supplydate = "";

    ContractTemplate.prototype.GetJson = function () {
        return JSON.stringify({
            "Contract": this.contract,
            "DateContract": this.contractdate,
            "Waybill": this.waybill,
            "DateWaybill": this.waybilldate,
            "Supplier": this.supplier,
            "DateReceived": this.supplydate
        })
    }

    ContractTemplate.prototype.GetAutoElem = function () {
        return $("<div>")
            .addClass("auto-elem")
            .attr("data-contract", this.contract)
            .attr("data-contractdate", this.contractdate)
            .attr("data-waybill", this.waybill)
            .attr("data-waybilldate", this.waybilldate)
            .attr("data-supplier", this.supplier)
            .attr("data-supplydate", this.supplydate)
            .append($("<div>").append(
                $("<p>")
                    .append($("<b>").html("Накладная: "))
                    .append(this.waybill.length > 30 ? this.waybill.slice(0, 27) + "..." : this.waybill)
                    .append($("<b>").html("; Дата: "))
                    .append(this.waybilldate)
            ).append(
                $("<p>")
                    .append($("<b>").html("Поставщик: "))
                    .append(this.supplier.length > 30 ? this.supplier.slice(0, 27) + "..." : this.supplier)
                    .append($("<b>").html("; Дата: "))
                    .append(this.supplydate)
            ));
    }
}
ContractTemplate.FromElement = function (element) {
    var Result = new ContractTemplate();
    Result.contract = $(element).attr("data-contract");
    Result.contractdate = $(element).attr("data-contractdate");
    Result.waybill = $(element).attr("data-waybill");
    Result.waybilldate = $(element).attr("data-waybilldate");
    Result.supplier = $(element).attr("data-supplier");
    Result.supplydate = $(element).attr("data-supplydate");
    return Result;
}
ContractTemplate.FromInputs = function (form, contract, contractdate, waybill, waybilldate, supplier, supplydate) {
    var Result = new ContractTemplate();
    Result.contract = $(form).find($("#" + contract)).val();
    Result.contractdate = $(form).find($("#" + contractdate)).val();
    Result.waybill = $(form).find($("#" + waybill)).val();
    Result.waybilldate = $(form).find($("#" + waybilldate)).val();
    Result.supplier = $(form).find($("#" + supplier)).val();
    Result.supplydate = $(form).find($("#" + supplydate)).val();

    return Result;
}
ContractTemplate.FromJsonArray = function (jsonarr) {
    var Result = [];
    for (var ind = 0; ind < jsonarr.length; ind++) {
        Result.push(ContractTemplate.FromJson(jsonarr[ind]));
    }
    return Result;
}
ContractTemplate.FromJson = function (json) {
    var Result = new ContractTemplate();
    Result.contract = json.Contract;
    Result.contractdate = json.DateContract;
    Result.waybill = json.Waybill;
    Result.waybilldate = json.DateWaybill;
    Result.supplier = json.Supplier;
    Result.supplydate = json.DateReceived;
    return Result;
}