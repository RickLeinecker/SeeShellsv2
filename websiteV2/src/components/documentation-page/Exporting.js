import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Placeholder from '../../assets/seeshellsLogo-small.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
}

class Exporting extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Exporting</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The exporting feature allows users to export shellbags in a report format.
                </Typography>
                <img src={Placeholder} alt="shellbag-export" />
            </div>
        )
    }
}

export default withStyles(styles)(Exporting);