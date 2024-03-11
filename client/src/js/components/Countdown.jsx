import PropTypes from 'prop-types';
import React from 'react';


class Countdown extends React.Component {
  static propTypes = {
    time: PropTypes.number,
    onEnd: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      timeLeft: props.time,
      minutes: parseInt(props.time / 60, 10),
      seconds: parseInt(props.time % 60, 10) < 10 ? '0' + parseInt(props.time % 60, 10) : parseInt(props.time % 60, 10),
      fired: false,
    };
  }

  componentDidMount() {
    this.startTimer();
  }

  componentWillUnmount() {
    clearInterval(this.state.interval);
  }

  startTimer = () => {
    let interval = setInterval(function () {
      let timeLeft = this.state.timeLeft - 1;
      let minutes = parseInt(timeLeft / 60, 10);
      let seconds = parseInt(timeLeft % 60, 10);

      seconds = seconds < 10 ? '0' + seconds : seconds;

      this.setState({ timeLeft, minutes, seconds }, () => {
        if (timeLeft < 0 && !this.state.fired) {
          this.props.onEnd();
          this.setState({ fired: true });
        }
      });
    }.bind(this), 1000);
    this.setState({ interval });
  };

  render() {
    return (
      <span className="timer">
        { this.state.minutes > 0 && `${this.state.minutes}m`} {this.state.seconds}s
      </span>
    );
  }
}


export default Countdown;
