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
                    .append(this.name)
                    .append(" (" + ClassList.filter(x => x.Id == this.type)[0].ShortName + "); ")
                    .append($("<b>").html("Прекурсор: "))
                    .append(this.precursor == true ? "Да; " : "Нет; ")
                    .append($("<b>").html("Ед.изм.: "))
                    .append(UnitList.filter(x => x.Id == this.type)[0].ShortName)
            ).append(
                $("<p>")
                    .append($("<b>").html("№ стд.: "))
                    .append((this.stdnumber.length > 10 ? this.stdnumber.slice(0, 8) + "..." : this.stdnumber) + "; ")
                    .append($("<b>").html("Рекв.метод.исп.: "))
                    .append(this.stdinfo.length > 20 ? this.stdinfo.slice(0, 18) + "..." : this.stdinfo)
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