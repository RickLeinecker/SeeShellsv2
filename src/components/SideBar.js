import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, BrowserRouter as Router } from 'react-router-dom';

const styles = {
};

class SideBar extends React.Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);
    }

    handleClick(event) {
        this.props.history.push("/" + event.currentTarget.id);
    }

    render() {
        return(
            <Router>

            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(SideBar));