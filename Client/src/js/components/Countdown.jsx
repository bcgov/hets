import React from 'react';

var Countdown = React.createClass({
  propTypes: {
    time: React.PropTypes.number,
    onEnd: React.PropTypes.func,
  },

  getInitialState() {
    return {
      timeLeft: this.props.time,
      minutes: parseInt(this.props.time / 60, 10),
      seconds: parseInt(this.props.time % 60, 10) < 10 ? '0' + parseInt(this.props.time % 60, 10) : parseInt(this.props.time % 60, 10),
      fired: false,
    };
  },

  componentDidMount() {
    this.startTimer();
  },

  componentWillUnmount() {
    clearInterval(this.state.interval);
  },

  startTimer() {
    var timeLeft = this.state.timeLeft;
    var interval = setInterval(function () {
      var minutes = parseInt(timeLeft / 60, 10);
      var seconds = parseInt(timeLeft % 60, 10);

      seconds = seconds < 10 ? '0' + seconds : seconds;

      this.setState({ minutes, seconds });

      if (--timeLeft < 0 && !this.state.fired) {
        this.props.onEnd();
        this.setState({ fired: true });
      }
    }.bind(this), 1000);
    this.setState({ interval });
  },

  render() {
    return (
      <span className="timer">
        { this.state.minutes > 0 && `${this.state.minutes}m`} {this.state.seconds}s
      </span>
    );
  },
});


export default Countdown;
