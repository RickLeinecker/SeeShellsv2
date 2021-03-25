import React from 'react';
import { withStyles } from '@material-ui/core/styles';

const styles = {
    footer: {
        backgroundColor: '#212121',
        width: '100%',
        display: 'flex',
        margin: '0px',
        height: '50px',
        justifyContent: 'right',
    },
    content: {
        margin: '15px',
    }
};

/*
*   Footer.js
*   - renders a simple footer bar
*/
class Footer extends React.Component {
    render() {
        return(
            <div className={this.props.classes.footer}>
                <div className={this.props.classes.content}>
                    Sponsored by Rick Leinecker | 2021
                </div>
            </div> 
        );
    }
}

export default withStyles(styles)(Footer);