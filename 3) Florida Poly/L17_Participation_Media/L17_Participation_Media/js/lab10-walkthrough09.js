/*Add code here*/
$(function() {
	$("#consume").click(function() {
        var url = "https://www.randyconnolly.com/funwebdev/services/travel/cities.php";
        $("#results").html("<ul></ul>");
        $.get(url, function (data, status) {
                $.each(data, function(index,value) {
                    var li = $('<li/>').html(value.name);
                    li.appendTo("div#results ul");                
                });                
            });
        });
    });
    