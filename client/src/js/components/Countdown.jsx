import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';

const Countdown = ({ time, onEnd }) => {
  const [timeLeft, setTimeLeft] = useState(time);
  const [minutes, setMinutes] = useState(Math.floor(time / 60));
  const [seconds, setSeconds] = useState(time % 60 < 10 ? '0' + (time % 60) : time % 60);
  const [fired, setFired] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setTimeLeft((prevTime) => {
        const newTimeLeft = prevTime - 1;
        const newMinutes = Math.floor(newTimeLeft / 60);
        const newSeconds = newTimeLeft % 60 < 10 ? '0' + (newTimeLeft % 60) : newTimeLeft % 60;

        setMinutes(newMinutes);
        setSeconds(newSeconds);

        if (newTimeLeft < 0 && !fired) {
          onEnd();
          setFired(true);
        }

        return newTimeLeft;
      });
    }, 1000);

    return () => clearInterval(interval);
  }, [fired, onEnd]);

  return (
    <span className="timer">
      {minutes > 0 && `${minutes}m`} {seconds}s
    </span>
  );
};

Countdown.propTypes = {
  time: PropTypes.number,
  onEnd: PropTypes.func,
};

export default Countdown;
