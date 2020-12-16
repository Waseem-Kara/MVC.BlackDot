var sourceSummaries = new Array();
Search = {
    SearchByTerm: function (searchTerm, url, reset = false) {
        //Scroll to top of the page on search
        $('html, body').animate({ scrollTop: 0 }, 'fast');

        //reset to first page
        if (reset === true) {
            ResetPageNumber();
            sourceSummaries = new Array();
        }
            

        //Show spinner
        $(".overlay").css("display", "block" );

        var startFrom = $("#pageNumber").text();
        $.ajax({
            url: url,
            type: "GET",
            data: {searchTerm, startFrom},
            dataType: "html"
        }).done(function(response) {
            $("#searchResults").html(response);

            //init bootstrap tooltips
            $('[data-toggle="tooltip"]').tooltip();

            //enable pagination nav
            $("#paginationNav").removeAttr("hidden");

            //after loading hide spinner
            $(".overlay").css("display", "none");
            
        }).fail(function(xhr) {
            //after loading hide spinner
            $(".overlay").css("display", "none");
        });


    },
    PreviousPage: function (searchTerm, url) {
        
        var updatedPageNumber = +$("#pageNumber").text() - 1;
        $("#pageNumber").text(updatedPageNumber);

        Search.SearchByTerm(searchTerm, url);

        //remove previous function when on page 1
        if (updatedPageNumber == 1) {
            $("#previousPageLink").addClass("disabled");
        }
    },
    NextPage: function (searchTerm, url) {
        
        $("#previousPageLink").removeClass("disabled");

        var updatedPageNumber = +$("#pageNumber").text() + 1;
        $("#pageNumber").text(updatedPageNumber);

        Search.SearchByTerm(searchTerm, url);
    },
    AddToSummary: function ($row) {
        $("#summarySubmit").removeAttr("hidden");

        var summaryObj = {};

        summaryObj.Title = $row.find(".source-title").val();
        summaryObj.Link = $row.find(".source-url").find("a").attr("href").val();
        summaryObj.Provider = $row.find(".source-provider").val();
        summaryObj.Description = $row.find(".source-summary").val();
        sourceSummaries.push(summaryObj);

     
    },
    DownloadSummary: function (url) {
     
     $.ajax({
             type: "POST",
             url: url,
             data: JSON.stringify(sourceSummaries),
         contentType: "application/json",
 
         dataType: "json",
            success: function (r) {
                
            },
            error: function () {


            }
        });
    }
}

function ResetPageNumber() {
    $("#previousPageLink").addClass("disabled");
    $("#pageNumber").text("1");
}