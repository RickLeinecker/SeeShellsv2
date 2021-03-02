import React from 'react';
import croppedLogo from '../assets/croppedLogo.png';
import croppedV2Logo from '../assets/croppedV2Logo.png';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';

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
        maxWidth: '400px',
        minHeight: '200px',
    },
    backgroundV1: {
        backgroundImage: 'linear-gradient(180deg, #424242 0%, rgba(255, 255, 255, 0) 100%), url(' + croppedLogo + ')',
        backgroundSize: 'cover',
        backgroundRepeat: 'no-repeat',
        width: '100%',
        height: '100%',
    },
    backgroundV2: {
        backgroundImage: 'linear-gradient(180deg, #424242 0%, rgba(255, 255, 255, 0) 100%), url(' + croppedV2Logo + ')',
        backgroundSize: 'cover',
        backgroundRepeat: 'no-repeat',
        width: '100%',
        height: '100%',
    },
    devTitle: {
        display: 'flex',
        justifyContent: 'right',
        textAlign: 'right',
        overflowWrap: 'break-word',
        color: '#33A1FD',
        marginTop: '10%',
        fontFamily: 'Georgia',
        fontWeight: 'bold',
    },
    devDescription: {
        display: 'flex',
        justifyContent: 'right',
        textAlign: 'right',
        overflowWrap: 'break-word',
    },
};

class DeveloperProfile extends React.Component {
    render() {
        return(
            <Card className={this.props.classes.devContainer}>
                {this.props.version === 1 &&
                    <div className={this.props.classes.backgroundV1}>
                        <CardContent>
                            <Typography variant="h5" className={this.props.classes.devTitle}>{this.props.name}</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.devDescription}>{this.props.role}</Typography>
                        </CardContent>
                    </div>
                }
                {this.props.version === 2 &&
                    <div className={this.props.classes.backgroundV2}>
                        <CardContent>
                            <Typography variant="h5" className={this.props.classes.devTitle}>{this.props.name}</Typography>
                            <Typography variant="subtitle1" className={this.props.classes.devDescription}>{this.props.role}</Typography>
                        </CardContent>
                    </div>
                }
            </Card>
        );
    }
}

export default withStyles(styles)(DeveloperProfile);