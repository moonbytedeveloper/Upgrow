

let hours = 24;
let minutes = 0;
let seconds = 0;

let totalSeconds = hours * 3600 + minutes * 60 + seconds;

// Elements
const hoursBlock = document.getElementById("hoursBlock");
const hoursColon = document.getElementById("hoursColon");

const hrUp = document.getElementById("hrUp");
const hrDown = document.getElementById("hrDown");
const minUp = document.getElementById("minUp");
const minDown = document.getElementById("minDown");
const secUp = document.getElementById("secUp");
const secDown = document.getElementById("secDown");

function pad(n) {
  return n < 10 ? "0" + n : n;
}

function flip(elUp, elDown, current, next) {
  elUp.textContent = pad(current);
  elDown.textContent = pad(next);

  elUp.classList.remove("flip-up");
  elDown.classList.remove("flip-down");

  void elUp.offsetWidth;

  elUp.classList.add("flip-up");
  elDown.classList.add("flip-down");
}

function updateHourVisibility(h) {
  if (h <= 0) {
    hoursBlock.style.display = "";
    hoursColon.style.display = "";
  } else {
    hoursBlock.style.display = "block";
    hoursColon.style.display = "inline";
  }
}

function tick() {
  if (totalSeconds <= 0) {
    clearInterval(timer);
    return;
  }

  const h = Math.floor(totalSeconds / 3600);
  const m = Math.floor((totalSeconds % 3600) / 60);
  const s = totalSeconds % 60;

  const nextH = Math.floor((totalSeconds - 1) / 3600);
  const nextM = Math.floor(((totalSeconds - 1) % 3600) / 60);
  const nextS = (totalSeconds - 1) % 60;

  updateHourVisibility(h);

  if (h > 0 && m === 0 && s === 0) {
    flip(hrUp, hrDown, h, nextH);
  }

  if (s === 0) {
    flip(minUp, minDown, m, nextM);
  }

  flip(secUp, secDown, s, nextS);

  totalSeconds--;
}

updateHourVisibility(hours);
const timer = setInterval(tick, 1000);



