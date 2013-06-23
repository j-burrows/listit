

function chartEmpty() {
    var canvas = document.getElementById("chartCanvas");
    var context = canvas.getContext("2d");
    var x = canvas.height / 2;
    var y = canvas.width / 2;
    var radius = 0.6 * ((x > y) ? y : x);
    var fontSize = 20;
    context.beginPath();
    context.moveTo(x, y);

    // line 1
    context.arc(x, y, radius, 0, 2 * Math.PI, false);
    context.lineWidth = 0.5;
    context.strokeStyle = "#40D040";
    context.fillStyle = "#40D040";
    context.fill();
    context.stroke();

    context.font = "bold "+fontSize+"px sans-serif";
    context.fillText("Current list is empty, edit list to add items.",0,20);
}

function chartSlice(startAngle, endAngle, color, itemName, price, quantity) {
    var textAngle = (startAngle + endAngle) / 2;

    var canvas = document.getElementById("chartCanvas");
    var context = canvas.getContext("2d");
    var x = canvas.height / 2;
    var y = canvas.width / 2;
    var radius = 0.6 * ((x > y) ? y : x);
    var fontSize = 15, lineSpacing = 18 ;
    context.beginPath();
    context.moveTo(x,y);


    context.arc(x, y, radius, startAngle, endAngle, false);

    context.lineWidth = 0.5;
    context.strokeStyle = "#333";
    context.fillStyle = color;
    context.fill();
    context.stroke();

    context.font = "bold " + fontSize + "px sans-serif";
    var textX = (x+ (Math.cos(textAngle) * (radius * 1.4)))-itemName.length*3;
    var textY = y + (Math.sin(textAngle) * (radius * 1.4) - lineSpacing);

    context.fillText(itemName, textX, textY);

    textY += lineSpacing;
    context.fillText("$"+price + " ("+quantity+")", textX, textY);

    textY += lineSpacing;
    context.fillText("$"+price*quantity+" total", textX, textY);
}

function chartPie(priceArray, quantityArray,names) {
    var color = ["#008BD6", "#FF8040", "#40D040","#E02000"];
    var radians = Math.PI * 2, start, end;
    var totalCost=0, costY = 20;
    var i;

    if(priceArray.length==0){
        chartEmpty();
        costY += 20;
    }

    for(i=0; i<priceArray.length;i++){
        totalCost+=priceArray[i]*quantityArray[i];
    }
    
    end = Math.PI/2;
    if (totalCost != 0) {
        for (i = 0; i < priceArray.length; i++) {
            start = end;
            end += radians * priceArray[i] * quantityArray[i] / totalCost;

            chartSlice(start, end, color[i%color.length],names[i],priceArray[i],quantityArray[i]);
        }
    }


    var canvas = document.getElementById("chartCanvas");
    var context = canvas.getContext("2d");
    var x = canvas.height / 2;
    var y = canvas.width / 2;
    var radius = 0.6 * ((x > y) ? y : x);
    var fontSize = 20;

   
    context.strokeStyle = "#40D040";
    context.fillStyle = "#40D040";

    context.font = "bold " + fontSize + "px sans-serif";
    context.fillText("Total cost: $"+totalCost, 0, costY);
}
