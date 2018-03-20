export const setInterval = () => {
  clearInterval(timer);
  var timer = window.setInterval(alert('hello'), 3000);  
  return timer;
};
