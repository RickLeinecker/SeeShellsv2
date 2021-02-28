import React from 'react';
import oldLogo from '../assets/oldLogo.png';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Card from '@material-ui/core/Card';

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
        minWidth: '200px',
    },
    logo: {
        width: '30%',
    },
    dev: {
        display: 'flex',
        justifyContent: 'center',
        overflowWrap: 'break-word',
    },
};

class DeveloperProfile extends React.Component {
    render() {
        return(
            <Card className={this.props.classes.devContainer}>
                <img src={oldLogo} alt='SeeShells Logo' className={this.props.classes.logo}/>
                <Typography variant="subtitle1" className={this.props.classes.dev}>{this.props.name}</Typography>
                <Typography variant="subtitle2" className={this.props.classes.dev}>{this.props.role}</Typography>
            </Card>
        );
    }
}

export default withStyles(styles)(DeveloperProfile);