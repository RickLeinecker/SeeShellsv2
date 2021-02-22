import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter, BrowserRouter as Router } from 'react-router-dom';

const styles = {
    sidebarContainer: {
        width: '20%',
        height: '100%',
        backgroundColor: '#424242',
        margin: '0px',
        display: 'flex',
        justifyContent: 'center',
    },
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
                <div className={this.props.classes.sidebarContainer}>
                    <p> ajksdhga9dughaidgahugikahksjghasjkg</p>
                </div>
            </Router>
        );
    }
}

export default withStyles(styles)(withRouter(SideBar));