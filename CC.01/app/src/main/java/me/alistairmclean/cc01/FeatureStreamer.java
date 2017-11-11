package me.alistairmclean.cc01;
import android.os.AsyncTask;

import java.io.ByteArrayOutputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * Written by David Singleton.
 * File recreated on 11/3/2017.
 */

public class FeatureStreamer extends Thread {
    private Socket sock;
    private DataOutputStream dos;
    private boolean mIsConnected = false;


    FeatureStreamer() {
    }

    public Socket getSocket() {return sock;}
    void connect(String addr, int port) {
        try {
            sock = new Socket(addr, port);
            dos = new DataOutputStream(sock.getOutputStream());
            mIsConnected = true;
        } catch (UnknownHostException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    void sendByteArray(byte[] send) throws IOException {
        if (dos != null) {
            dos.writeInt(send.length);
            dos.write(send, 0, send.length);
            dos.flush();
        }
    }
    boolean isConnected() {
        return mIsConnected;
    }

    void sendFeatures(int width, int height, byte[] send) {
        try {
            if (dos != null) {
                dos.writeInt(width);
                dos.writeInt(height);
                dos.write(send, 0, send.length);
                dos.flush();
            }
        } catch (IOException e) {
        }
    }

    void close() throws IOException {
        if (dos != null) {
            dos.close();
        }
        if (sock != null) {
            sock.close();
        }
        mIsConnected = false;
    }





}
