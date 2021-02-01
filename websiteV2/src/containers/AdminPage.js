import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';

const styles = {
    loginContainer: {
        color: 'white',
    },
}

class AdminPage extends React.Component {
    render() {
        return(
            <div className={this.props.classes.loginContainer}>
                
            </div>
        );
    }
}

export default withStyles(styles)(withRouter(AdminPage));