import React from 'react';
import oldLogo from '../assets/oldLogo.png';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';

const styles = {
    devContainer: {
        backgroundColor: '#424242',
        margin: '10px',
        width: '20%',
        height: '30%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        flexDirection: 'column',
        color: 'white',
    },
    logo: {
        width: '30%',
    },
};

class DeveloperProfile extends React.Component {
    render() {
        return(
            <div className={this.props.classes.devContainer}>
                <img src={oldLogo} alt='SeeShells Logo' className={this.props.classes.logo}/>
                <Typography variant="subtitle1">{this.props.name}</Typography>
                <Typography variant="subtitle2">{this.props.role}</Typography>
            </div>
        );
    }
}

export default withStyles(styles)(DeveloperProfile);