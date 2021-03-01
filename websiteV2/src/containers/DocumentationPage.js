import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';

const styles = {
    documentationPage: {
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

class DocumentationPage extends React.Component {
    render() {
        return(
            <Paper className={this.props.classes.documentationPage}>
                <Paper elevation={2} square={true} className={this.props.classes.sidebarContainer}>

                </Paper>
            </Paper>
        );
    }
}

export default withStyles(styles)(withRouter(DocumentationPage));