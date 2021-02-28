import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';

const styles = {
    aboutPage: {
        display: 'flex',
        height: '100%',
        width: '100%',
    },
    sidebarContainer: {
        width: '20%',
        height: '100%',
        backgroundColor: '#424242',
        margin: '0px',
        display: 'flex',
        justifyContent: 'center',
    },
};

class AboutPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.aboutPage}>
                <Paper elevation={2} square={true} className={this.props.classes.sidebarContainer}>

                </Paper>
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(AboutPage));